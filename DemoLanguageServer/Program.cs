//#define WAIT_FOR_DEBUGGER
//#define USE_CONSOLE_READER

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using JsonRpc.Standard;
using JsonRpc.Standard.Contracts;
using JsonRpc.Standard.Server;

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
            var rpcResolver = new RpcMethodResolver();
            rpcResolver.Register(typeof(Program).GetTypeInfo().Assembly);
            using (var cin = Console.OpenStandardInput())
            using (var cout = Console.OpenStandardOutput())
            using (var bcin = new BufferedStream(cin))
            using (var logWriter = File.CreateText("messages-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log"))
            {
                logWriter.AutoFlush = true;
                var sml = new MyStreamMessageLogger(logWriter);
                var writer = new PartwiseStreamMessageWriter(cout, sml);
#if USE_CONSOLE_READER
                using (var inreader = new StreamReader(bcin, Encoding.UTF8, false, 4096, true))
                {
                    var reader = new ByLineTextMessageReader(inreader);
#else
                {
                    var reader = new PartwiseStreamMessageReader(bcin, sml);

#endif
                    var host = new JsonRpcServiceHost(reader, writer, rpcResolver,
                        JsonRpcServiceHostOptions.ConsistentResponseSequence);
                    host.RunAsync().Wait();
                }
                logWriter.WriteLine("Exited");
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