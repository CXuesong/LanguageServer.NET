using System;
using System.Collections.Generic;
using System.Text;
using JsonRpc.DynamicProxy.Client;
using JsonRpc.Standard.Client;

namespace LanguageServer.VsCode.Contracts.Client
{
    /// <summary>
    /// A class that puts all the Language Protocol client-side proxy methods together.
    /// </summary>
    public class ClientProxy
    {

        /// <summary>
        /// Initializes a client-side proxy method aggreator instance with specified
        /// <see cref="JsonRpcProxyBuilder"/> and <see cref="JsonRpcClient"/>.
        /// </summary>
        /// <param name="proxyBuilder">The builder used to build implementations for the stub interfaces.</param>
        /// <param name="rpcClient">The client used to send JSON RPC requests.</param>
        /// <exception cref="ArgumentNullException">Either <paramref name="proxyBuilder"/> or <paramref name="rpcClient"/> is <c>null</c>.</exception>
        public ClientProxy(JsonRpcProxyBuilder proxyBuilder, JsonRpcClient rpcClient)
        {
            if (proxyBuilder == null) throw new ArgumentNullException(nameof(proxyBuilder));
            if (rpcClient == null) throw new ArgumentNullException(nameof(rpcClient));
            ProxyBuilder = proxyBuilder;
            RpcClient = rpcClient;
            _Client = new Lazy<IClient>(() => ProxyBuilder.CreateProxy<IClient>(RpcClient));
            _Document = new Lazy<IDocument>(() => ProxyBuilder.CreateProxy<IDocument>(RpcClient));
            _Telemetry = new Lazy<ITelemetry>(() => ProxyBuilder.CreateProxy<ITelemetry>(RpcClient));
            _TextDocument = new Lazy<ITextDocument>(() => ProxyBuilder.CreateProxy<ITextDocument>(RpcClient));
            _Window = new Lazy<IWindow>(() => ProxyBuilder.CreateProxy<IWindow>(RpcClient));
            _Workspace = new Lazy<IWorkspace>(() => ProxyBuilder.CreateProxy<IWorkspace>(RpcClient));
        }

        /// <summary>
        /// The builder used to build implementations for the stub interfaces.
        /// </summary>
        public JsonRpcProxyBuilder ProxyBuilder { get; }

        /// <summary>
        /// The client used to send JSON RPC requests.
        /// </summary>
        public JsonRpcClient RpcClient { get; }


        private readonly Lazy<IClient> _Client;
        private readonly Lazy<IDocument> _Document;
        private readonly Lazy<ITelemetry> _Telemetry;
        private readonly Lazy<ITextDocument> _TextDocument;
        private readonly Lazy<IWindow> _Window;
        private readonly Lazy<IWorkspace> _Workspace;

        public IClient Client => _Client.Value;

        public IDocument Document => _Document.Value;

        public ITelemetry Telemetry => _Telemetry.Value;

        public ITextDocument TextDocument => _TextDocument.Value;

        public IWindow Window => _Window.Value;

        public IWorkspace Workspace => _Workspace.Value;

    }
}
