using System;
using System.Threading;
using JsonRpc.Standard.Client;
using JsonRpc.Standard.Contracts;
using JsonRpc.Standard.Server;
using LanguageServer.VsCode.Contracts.Client;

namespace DemoLanguageServer
{
    public class LanguageServerSession : Session
    {
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        public LanguageServerSession(JsonRpcClient rpcClient, IJsonRpcContractResolver contractResolver)
        {
            if (rpcClient == null) throw new ArgumentNullException(nameof(rpcClient));
            RpcClient = rpcClient;
            var builder = new JsonRpcProxyBuilder {ContractResolver = contractResolver};
            ClientWindow = builder.CreateProxy<IWindow>(rpcClient);
        }

        public CancellationToken CancellationToken => cts.Token;

        public JsonRpcClient RpcClient { get; }

        public IWindow ClientWindow { get; }

        public void StopServer()
        {
            cts.Cancel();
        }
        
    }
}