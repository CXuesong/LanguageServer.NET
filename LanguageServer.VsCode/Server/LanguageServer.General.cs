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
                    return new {Capabilities = e.ClientCapabilities};
                }
            ));
            requestHandlerDict.Add("shutdown", CreateHandler<GeneralRequestEventArgs>(e => OnShutdown()));
            requestHandlerDict.Add("exit", CreateHandler<GeneralRequestEventArgs>(e => OnExit()));
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
        /// The shutdown request is sent from the client to the server.
        /// It asks the server to shut down, but to not exit
        /// (otherwise the response might not be delivered correctly to the client).
        /// There is a separate exit notification that asks the server to exit.
        /// </summary>
        public event EventHandler Shutdown;

        /// <summary>
        /// Raises the <see cref="Shutdown"/> event.
        /// </summary>
        protected virtual void OnShutdown()
        {
            try
            {
                Shutdown?.Invoke(this, EventArgs.Empty);
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
        public event EventHandler Exit;

        /// <summary>
        /// Raises the <see cref="Exit"/> event.
        /// </summary>
        protected virtual void OnExit()
        {
            Exit?.Invoke(this, EventArgs.Empty);
        }
    }
}
