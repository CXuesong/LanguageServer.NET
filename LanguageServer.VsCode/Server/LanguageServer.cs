using System;
using System.Collections.Generic;
using System.Diagnostics;
using LanguageServer.VsCode.JsonRpc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.Server
{
    public partial class LanguageServer : IDisposable
    {

        public LanguageServer(IConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            Connection = connection;
            Connection.MessageReceived += MessageSource_MessageReceived;
        }

        private static Func<GeneralRequestMessage, object> CreateHandler<TEventArgs>(
            Action<TEventArgs> eventArgsHandler)
        {
            if (eventArgsHandler == null) throw new ArgumentNullException(nameof(eventArgsHandler));
            return CreateHandler<TEventArgs>(e =>
            {
                eventArgsHandler(e);
                return null;
            });
        }

        private static Func<GeneralRequestMessage, object> CreateHandler<TEventArgs>(Func<TEventArgs, object> eventArgsHandler)
        {
            if (eventArgsHandler == null) throw new ArgumentNullException(nameof(eventArgsHandler));
            return request =>
            {
                var e = request.Params.ToObject<TEventArgs>(RpcSerializer.Serializer);
                var response = eventArgsHandler(e);
                return response;
            };
        }

        private readonly Dictionary<string, Func<GeneralRequestMessage, object>> requestHandlerDict
            = new Dictionary<string, Func<GeneralRequestMessage, object>>();

        public IConnection Connection { get; }

        public bool IsDisposed { get;private set;}

        public bool IsInitialized { get; private set; }

        private int? exitCode;

        /// <summary>
        /// Starts the language server.
        /// </summary>
        /// <returns>The suggested exit code.</returns>
        public int Start()
        {
            return (int) Start(-1);
        }

        /// <summary>
        /// Starts the language server for a given duration.
        /// </summary>
        /// <param name="millisecondsDuration">The server stop listening after this duration. Specify -1 for infinite.</param>
        /// <returns>The suggested exit code, or <c>null</c> if the duration has been reached.</returns>
        public int? Start(int millisecondsDuration)
        {
            if (millisecondsDuration < -1) throw new ArgumentOutOfRangeException(nameof(millisecondsDuration));
            exitCode = null;
            Connection.Listen(millisecondsDuration);
            return exitCode;
        }

        private void MessageSource_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e.Message is GeneralRequestMessage request)
            {
                if (requestHandlerDict.TryGetValue(request.Method, out var handler))
                {
                    var response = handler(request);
                    e.Response = response;
                }
            }
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            IsDisposed = true;
            if (disposing)
            {
                Connection.MessageReceived -= MessageSource_MessageReceived;
            }
            // release unmanaged resources here
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        ~LanguageServer()
        {
            Dispose(false);
        }
    }
}
