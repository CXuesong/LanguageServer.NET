using System;
using System.Threading;
using JsonRpc.Standard.Client;
using JsonRpc.Standard.Contracts;
using JsonRpc.Standard.Server;
using LanguageServer.VsCode.Contracts.Client;
using LanguageServer.VsCode.Server;

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
            Client = new ClientProxy(builder, rpcClient);
            Documents = new TextDocumentCollection();
        }

        public CancellationToken CancellationToken => cts.Token;

        public JsonRpcClient RpcClient { get; }

        public ClientProxy Client { get; }

        public TextDocumentCollection Documents { get; }

        public void StopServer()
        {
            cts.Cancel();
        }
        
    }
}