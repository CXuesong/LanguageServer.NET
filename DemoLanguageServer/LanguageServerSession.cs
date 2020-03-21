using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JsonRpc.DynamicProxy.Client;
using JsonRpc.Client;
using JsonRpc.Contracts;
using JsonRpc.Server;
using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Contracts.Client;
using LanguageServer.VsCode.Server;

namespace DemoLanguageServer
{
    public class LanguageServerSession
    {
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        public LanguageServerSession(JsonRpcClient rpcClient, IJsonRpcContractResolver contractResolver)
        {
            RpcClient = rpcClient ?? throw new ArgumentNullException(nameof(rpcClient));
            var builder = new JsonRpcProxyBuilder {ContractResolver = contractResolver};
            Client = new ClientProxy(builder, rpcClient);
            Documents = new ConcurrentDictionary<Uri, SessionDocument>();
            DiagnosticProvider = new DiagnosticProvider();
        }

        public CancellationToken CancellationToken => cts.Token;

        public JsonRpcClient RpcClient { get; }

        public ClientProxy Client { get; }

        public ConcurrentDictionary<Uri, SessionDocument> Documents { get; }

        public DiagnosticProvider DiagnosticProvider { get; }

        public LanguageServerSettings Settings { get; set; } = new LanguageServerSettings();

        public void StopServer()
        {
            cts.Cancel();
        }

    }

    public class SessionDocument
    {
        /// <summary>
        /// Actually makes the changes to the inner document per this milliseconds.
        /// </summary>
        private const int RenderChangesDelay = 100;

        public SessionDocument(TextDocumentItem doc)
        {
            Document = TextDocument.Load<FullTextDocument>(doc);
        }

        private Task updateChangesDelayTask;

        private readonly object syncLock = new object();

        private List<TextDocumentContentChangeEvent> impendingChanges = new List<TextDocumentContentChangeEvent>();

        public event EventHandler DocumentChanged;

        public TextDocument Document { get; set; }

        public void NotifyChanges(IEnumerable<TextDocumentContentChangeEvent> changes)
        {
            lock (syncLock)
            {
                if (impendingChanges == null)
                    impendingChanges = changes.ToList();
                else
                    impendingChanges.AddRange(changes);
            }
            if (updateChangesDelayTask == null || updateChangesDelayTask.IsCompleted)
            {
                updateChangesDelayTask = Task.Delay(RenderChangesDelay);
                updateChangesDelayTask.ContinueWith(t => Task.Run((Action)MakeChanges));
            }
        }

        private void MakeChanges()
        {
            List<TextDocumentContentChangeEvent> localChanges;
            lock (syncLock)
            {
                localChanges = impendingChanges;
                if (localChanges == null || localChanges.Count == 0) return;
                impendingChanges = null;
            }
            Document = Document.ApplyChanges(localChanges);
            if (impendingChanges == null)
            {
                localChanges.Clear();
                lock (syncLock)
                {
                    if (impendingChanges == null)
                        impendingChanges = localChanges;
                }
            }
            OnDocumentChanged();
        }

        protected virtual void OnDocumentChanged()
        {
            DocumentChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}