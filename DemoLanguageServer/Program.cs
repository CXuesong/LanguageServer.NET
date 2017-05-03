//#define WAIT_FOR_DEBUGGER
//#define USE_CONSOLE_READER

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using JsonRpc.Standard;
using JsonRpc.Standard.Client;
using JsonRpc.Standard.Contracts;
using JsonRpc.Standard.Dataflow;
using JsonRpc.Standard.Server;

namespace DemoLanguageServer
{
    static class Program
    {
        static void Main(string[] args)
        {
#if WAIT_FOR_DEBUGGER
            while (!Debugger.IsAttached) Thread.Sleep(1000);
            Debugger.Break();
#endif
            using (var logWriter = File.CreateText("messages-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log"))
#if !USE_CONSOLE_READER
            using (var cin = Console.OpenStandardInput())
            using (var bcin = new BufferedStream(cin))
#endif
            using (var cout = Console.OpenStandardOutput())
            {
                logWriter.AutoFlush = true;
                var contractResolver = new JsonRpcContractResolver
                {
                    NamingStrategy = new CamelCaseJsonRpcNamingStrategy(),
                    ParameterValueConverter = new CamelCaseJsonValueConverter(),
                };
                var client = new JsonRpcClient();
                client.MessageSending += (_, e) =>
                {
                    lock (logWriter) logWriter.WriteLine("<C{0}", e.Message);
                };
                client.MessageReceiving += (_, e) =>
                {
                    lock (logWriter) logWriter.WriteLine(">C{0}", e.Message);
                };
                // Configure & build service host
                var session = new LanguageServerSession(client, contractResolver);
                var host = BuildServiceHost(session, logWriter, contractResolver);
                // Connect the datablocks
                var target = new PartwiseStreamMessageTargetBlock(cout);
#if USE_CONSOLE_READER

                var source = new ByLineTextMessageSourceBlock(Console.In);
#else
                var source = new PartwiseStreamMessageSourceBlock(bcin);
#endif
                using (host.Attach(source, target))
                    // We want to capture log all the server-to-client calls as well
                using (client.Attach(source, target))
                    // If we want server to stop, just stop the "source"
                using (session.CancellationToken.Register(() => source.Complete()))
                {
                    session.CancellationToken.WaitHandle.WaitOne();
                }
                logWriter.WriteLine("Exited");
            }
        }

        // DON'T USE IT. IT MAY SCREW UP.
        //private static ISourceBlock<T> Intercept<T>(this ISourceBlock<T> source, Action<T> action)
        //{
        //    var block = new TransformBlock<T, T>(i =>
        //    {
        //        action(i);
        //        return i;
        //    });
        //    source.LinkTo(block);
        //    return source;
        //}

        //private static ITargetBlock<T> Intercept<T>(this ITargetBlock<T> target, Action<T> action)
        //{
        //    var block = new TransformBlock<T, T>(i =>
        //    {
        //        action(i);
        //        return i;
        //    });
        //    block.LinkTo(target);
        //    return block;
        //}

        private static IJsonRpcServiceHost BuildServiceHost(ISession session, TextWriter logWriter, IJsonRpcContractResolver contractResolver)
        {
            var builder = new ServiceHostBuilder
            {
                ContractResolver = contractResolver,
                Session = session,
                Options = JsonRpcServiceHostOptions.ConsistentResponseSequence,
            };
            builder.Register(typeof(Program).GetTypeInfo().Assembly);
            // Log all the client-to-server calls.
            builder.Intercept(async (context, next) =>
            {
                lock (logWriter) logWriter.WriteLine("> {0}", context.Request);
                await next();
                lock (logWriter) logWriter.WriteLine("< {0}", context.Response);
            });
            return builder.Build();
        }
        
    }
}