using System;
using System.Collections.Generic;
using System.Text;
using LanguageServer.VsCode.JsonRpc;

namespace LanguageServer.VsCode.Server
{
    partial class LanguageServer
    {

        private void RegisterGeneralMessages()
        {
            requestHandlerDict.Add("initialize", CreateHandler<InitializingEventArgs>(e =>
                {
                    OnInitializing(e);
                    return new {Capabilities = e.ServerCapabilities};
                }
            ));
            requestHandlerDict.Add("initialized", CreateHandler<GeneralRequestEventArgs>(e => OnInitialized(e)));
            requestHandlerDict.Add("shutdown", CreateHandler<GeneralRequestEventArgs>(e => OnShutdown(e)));
            requestHandlerDict.Add("exit", CreateHandler<GeneralRequestEventArgs>(e => OnExit(e)));

        }

        /// <summary>
        /// The initialize request is sent as the first request from the client to the server. 
        /// </summary>
        public event EventHandler<InitializingEventArgs> Initializing;

        /// <summary>
        /// Raises the <see cref="Initializing"/> event.
        /// </summary>
        protected virtual void OnInitializing(InitializingEventArgs e)
        {
            Initializing?.Invoke(this, e);
            IsInitialized = true;
        }

        /// <summary>
        /// The initialized notification is sent from the client to the server after the client
        /// received the result of the initialize request but before the client is sending
        /// any other request or notification to the server. The server can use the initialized
        /// notification for example to dynamically register capabilities.
        /// </summary>
        public event EventHandler<GeneralRequestEventArgs> Initialized;

        /// <summary>
        /// Raises the <see cref="Initialized"/> event.
        /// </summary>
        protected virtual void OnInitialized(GeneralRequestEventArgs e)
        {
            Initialized?.Invoke(this, e);
        }

        /// <summary>
        /// The shutdown request is sent from the client to the server.
        /// It asks the server to shut down, but to not exit
        /// (otherwise the response might not be delivered correctly to the client).
        /// There is a separate exit notification that asks the server to exit.
        /// </summary>
        public event EventHandler<GeneralRequestEventArgs> Shutdown;

        /// <summary>
        /// Raises the <see cref="Shutdown"/> event.
        /// </summary>
        protected virtual void OnShutdown(GeneralRequestEventArgs e)
        {
            try
            {
                Shutdown?.Invoke(this, e);
            }
            finally
            {
                IsInitialized = false;
            }
        }

        /// <summary>
        /// A notification to ask the server to exit its process.
        /// </summary>
        /// <remarks>The server should exit with success code 0 if the shutdown request has been received before;
        /// otherwise with error code 1.</remarks>
        public event EventHandler<GeneralRequestEventArgs> Exit;

        /// <summary>
        /// Raises the <see cref="Exit"/> event.
        /// </summary>
        protected virtual void OnExit(GeneralRequestEventArgs e)
        {
            Exit?.Invoke(this, e);
        }
    }
}
