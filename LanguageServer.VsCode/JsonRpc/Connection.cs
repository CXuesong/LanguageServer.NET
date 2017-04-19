using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.JsonRpc
{
    public class Connection : IConnection
    {
        private readonly MessageReader _MessageReader;
        private readonly MessageWriter _MessageWriter;
        private volatile bool _IsListening;
        private readonly object syncLock = new object();
        private volatile CancellationTokenSource listenCts;

        /// <inheritdoc />
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public Connection(MessageReader messageReader, MessageWriter messageWriter)
        {
            if (messageReader == null) throw new ArgumentNullException(nameof(messageReader));
            if (messageWriter == null) throw new ArgumentNullException(nameof(messageWriter));
            _MessageReader = messageReader;
            _MessageWriter = messageWriter;
        }

        /// <summary>
        /// Creates a <see cref="Connection"/> from <see cref="Stream"/>s.
        /// </summary>
        public static Connection FromStreams(Stream inStream, Stream outStream)
        {
            return FromStreams(inStream, outStream, null);
        }

        /// <summary>
        /// Creates a <see cref="Connection"/> from <see cref="Stream"/>s.
        /// </summary>
        public static Connection FromStreams(Stream inStream, Stream outStream, IStreamMessageLogger logger)
        {
            if (inStream == null) throw new ArgumentNullException(nameof(inStream));
            if (outStream == null) throw new ArgumentNullException(nameof(outStream));
            return new Connection(new StreamMessageReader(inStream, logger), new StreamMessageWriter(outStream, logger));
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

        /// <inheritdoc />
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
                        Exception handlerException = null;
                        try
                        {
                            OnMessageReceived(e);
                        }
                        catch (Exception ex)
                        {
                            handlerException = ex;
                        }
                        if (e.Message is RequestMessage)
                        {
                            _MessageWriter.Write(e.CreateResponseMessage(handlerException));
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
                listenCts?.Cancel();
            }
        }

        /// <summary>
        /// Raises the <see cref="MessageReceived"/> event.
        /// </summary>
        protected virtual void OnMessageReceived(MessageReceivedEventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
}
