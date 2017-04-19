using System;
using System.Diagnostics;
using LanguageServer.VsCode.JsonRpc;
using LanguageServer.VsCode.Server;
using LangServer = LanguageServer.VsCode.Server.LanguageServer;

namespace DemoLanguageServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var cin = Console.OpenStandardInput())
            using (var cout = Console.OpenStandardOutput())
            {
                var connection = Connection.FromStreams(cin, cout, new MyStreamMessageLogger());
                using (var server = new LangServer(connection))
                {
                    server.Start();
                }
            }
        }
    }

    class MyStreamMessageLogger : IStreamMessageLogger
    {
        /// <inheritdoc />
        public void NotifyMessageSent(string content)
        {
            var message = "< " + content;
            Trace.WriteLine(message);
        }

        /// <inheritdoc />
        public void NotifyMessageReceived(string content)
        {
            var message = "> " + content;
            Trace.WriteLine(message);
        }
    }
}