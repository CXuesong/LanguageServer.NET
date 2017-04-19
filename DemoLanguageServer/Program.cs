//#define WAIT_FOR_DEBUGGER

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using LanguageServer.VsCode.JsonRpc;
using LanguageServer.VsCode.Server;
using LangServer = LanguageServer.VsCode.Server.LanguageServer;

namespace DemoLanguageServer
{
    class Program
    {
        static void Main(string[] args)
        {
#if WAIT_FOR_DEBUGGER
            while (!Debugger.IsAttached) Thread.Sleep(1000);
            Debugger.Break();
#endif
            using (var cin = Console.OpenStandardInput())
            using (var cout = Console.OpenStandardOutput())
            using (var logWriter = File.CreateText("messages-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log"))
            {
                logWriter.AutoFlush = true;
                var connection = Connection.FromStreams(cin, cout, new MyStreamMessageLogger(logWriter));
                using (var server = new LangServer(connection))
                {
                    server.Start();
                }
            }
        }
    }

    class MyStreamMessageLogger : IStreamMessageLogger
    {
        public TextWriter Writer { get; }

        public MyStreamMessageLogger(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            Writer = writer;
        }

        /// <inheritdoc />
        public void NotifyMessageSent(string content)
        {
            Writer.Write("< ");
            Writer.WriteLine(content);
        }

        /// <inheritdoc />
        public void NotifyMessageReceived(string content)
        {
            Writer.Write("> ");
            Writer.WriteLine(content);
        }
    }
}