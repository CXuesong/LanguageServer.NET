using System;
using System.Threading;
using System.Threading.Tasks;

namespace LanguageServer.VsCode.JsonRpc
{
    public class Connection : IMessageSource
    {
        private readonly MessageReader _MessageReader;
        private readonly MessageWriter _MessageWriter;
        private volatile bool _IsListening;
        private readonly object syncLock = new object();
        private volatile CancellationTokenSource listenCts;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public Connection(MessageReader messageReader, MessageWriter messageWriter)
        {
            if (messageReader == null) throw new ArgumentNullException(nameof(messageReader));
            if (messageWriter == null) throw new ArgumentNullException(nameof(messageWriter));
            _MessageReader = messageReader;
            _MessageWriter = messageWriter;
        }

        /// <summary>
        /// Determines whether the connection is listening.
        /// </summary>
        public bool IsListening
        {
            get
            {
                lock (syncLock) return _IsListening;
            }
        }

        public void Listen()
        {
            Listen(-1);
        }

        public void Listen(int millisecondsTimeout)
        {
            if (millisecondsTimeout < -1) throw new ArgumentOutOfRangeException(nameof(millisecondsTimeout));
            lock (syncLock)
            {
                if (_IsListening) throw new InvalidOperationException("Already listening on the connection.");
                _IsListening = true;
                listenCts = new CancellationTokenSource(millisecondsTimeout);
            }
            var ct = listenCts.Token;
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        var message = _MessageReader.Read();
                        var e = new MessageReceivedEventArgs(message);
                        OnMessageReceived(e);
                        if (e.Response != null)
                        {
                            _MessageWriter.Write(e.Response);
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                }
            }
            finally
            {
                lock (syncLock)
                {
                    listenCts.Dispose();
                    listenCts = null;
                    _IsListening = false;
                }
            }
        }

        public void StopListening()
        {
            lock (syncLock)
            {
                _IsListening = false;
                listenCts?.Cancel();
            }
        }

        protected virtual void OnMessageReceived(MessageReceivedEventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
}
