using System;
using System.Diagnostics;
using LanguageServer.VsCode.JsonRpc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.Server
{
    public class LanguageServer
    {
        public LanguageServer()
        {
        }

        #region General Messages

        /// <summary>
        /// The initialize request is sent as the first request from the client to the server. 
        /// </summary>
        public event EventHandler<InitializingEventArgs> Initializing;

        protected virtual void OnInitializing(InitializingEventArgs e)
        {
            Initializing?.Invoke(this, e);
            IsInitialized = true;
        }

        #endregion

        /// <summary>
        /// Attaches the language server to a message source.
        /// </summary>
        public IDisposable Attach(IMessageSource messageSource)
        {
            if (messageSource == null) throw new ArgumentNullException(nameof(messageSource));
            messageSource.MessageReceived += MessageSource_MessageReceived;
            return new AttachDisposable(this, messageSource);
        }

        public bool IsInitialized { get; private set; }

        private void MessageSource_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e.Message is RequestMessage request)
            {
                switch (request.Method)
                {
                    case "initialize":
                        var e1 = request.Params.ToObject<InitializingEventArgs>(RpcSerializer.Serializer);
                        OnInitializing(e1);
                        e.Response = new ResponseMessage(new {Capabilities = e1.ClientCapabilities});
                        break;
                    default:
                        break;
                }
            }
        }

        private class AttachDisposable : IDisposable
        {
            private readonly LanguageServer _Owner;
            private readonly IMessageSource _MessageSource;

            public AttachDisposable(LanguageServer owner, IMessageSource messageSource)
            {
                _Owner = owner;
                _MessageSource = messageSource;
                Debug.Assert(owner != null);
                Debug.Assert(messageSource != null);

            }

            /// <inheritdoc />
            public void Dispose()
            {
                _MessageSource.MessageReceived -= _Owner.MessageSource_MessageReceived;
            }
        }
    }

}
