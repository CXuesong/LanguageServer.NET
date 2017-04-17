using System;
using LanguageServer.VsCode.JsonRpc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.Server
{
    public class LanguageServer : IDisposable
    {
        private readonly IMessageSource _MessageSource;

        //public event EventHandler<>

        public LanguageServer(IMessageSource messageSource)
        {
            _MessageSource = messageSource;
            _MessageSource.MessageReceived += MessageSource_MessageReceived;
        }

        private void MessageSource_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _MessageSource.MessageReceived -= MessageSource_MessageReceived;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
