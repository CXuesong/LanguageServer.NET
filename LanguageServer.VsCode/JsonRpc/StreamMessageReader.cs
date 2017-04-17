using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LanguageServer.VsCode.JsonRpc
{
    /// <summary>
    /// Reads JSON RPC messages from a Stream.
    /// </summary>
    public class StreamMessageReader : MessageReader
    {
        private const int headerBufferSize = 1024;
        private const int contentBufferSize = 4 * 1024;

        private static readonly byte[] headerTerminationSequence = {0x0d, 0x0a, 0x0d, 0x0a};


        public StreamMessageReader(Stream stream) : this(stream, Encoding.UTF8, null)
        {

        }

        public StreamMessageReader(Stream stream, IStreamMessageLogger messageLogger) : this(stream, Encoding.UTF8,
            messageLogger)
        {

        }

        public StreamMessageReader(Stream stream, Encoding encoding, IStreamMessageLogger messageLogger)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            BaseStream = stream;
            Encoding = encoding;
            MessageLogger = messageLogger;
        }

        public IStreamMessageLogger MessageLogger { get; }

        public Stream BaseStream { get; }

        public Encoding Encoding { get; }

        public override Message Read()
        {
            var headerBuffer = new List<byte>(headerBufferSize);
            int termination;
            int contentLength;
            {
                var headerSubBuffer = new byte[headerBufferSize];
                while ((termination = headerBuffer.IndexOf(headerTerminationSequence)) < 0)
                {
                    var readLength = BaseStream.Read(headerSubBuffer, 0, headerBufferSize);
                    if (readLength == 0)
                    {
                        if (headerBuffer.Count == 0)
                            return null; // EOF
                        else
                            throw new JsonRpcException("Unexpected EOF when reading header.");
                    }
                    headerBuffer.AddRange(headerSubBuffer.Take(readLength));
                }
                // Read header.
                var headerBytes = new byte[termination];
                headerBuffer.CopyTo(0, headerBytes, 0, termination);
                var header = Encoding.GetString(headerBytes, 0, termination);
                var headers = header
                    .Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None)
                    .Select(s => s.Split(new[] {": "}, 2, StringSplitOptions.None));
                try
                {
                    contentLength = Convert.ToInt32(headers.First(e => e[0] == "Content-Length")[1]);
                }
                catch (InvalidOperationException)
                {
                    throw new JsonRpcException("Invalid JSON RPC header. Content-Length is missing.");
                }
                catch (FormatException)
                {
                    throw new JsonRpcException("Invalid JSON RPC header. Content-Length is invalid.");
                }
                if (contentLength <= 0)
                    throw new JsonRpcException("Invalid JSON RPC header. Content-Length is invalid.");
            }
            // Concatenate and read the rest of the content.
            var contentBuffer = new byte[contentLength];
            var contentOffset = termination + headerTerminationSequence.Length;
            headerBuffer.CopyTo(contentOffset, contentBuffer, 0, headerBuffer.Count - contentOffset);
            var pos = headerBuffer.Count - contentOffset; // The position to put the next character.
            while (pos < contentLength)
            {
                var length = BaseStream.Read(contentBuffer, pos, Math.Min(contentLength - pos, contentBufferSize));
                if (length == 0) throw new JsonRpcException("Unexpected EOF when reading content.");
                pos += length;
            }
            // Deserialization
            using (var ms = new MemoryStream(contentBuffer))
            {
                using (var sr = new StreamReader(ms, Encoding))
                {
                    if (MessageLogger != null)
                    {
                        var content = sr.ReadToEnd();
                        MessageLogger.NotifyMessageReceived(content);
                        return RpcSerializer.DeserializeMessage(content);
                    }
                    else
                    {
                        return RpcSerializer.DeserializeMessage(sr);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Provides logger of messages for diagnostic purpose.
    /// </summary>
    public interface IStreamMessageLogger
    {
        void NotifyMessageSent(string content);

        void NotifyMessageReceived(string content);
    }

    public class StreamMessageLogger : IStreamMessageLogger
    {
        private readonly Action<string> _NotifyMessageSent;
        private readonly Action<string> _NotifyMessageReceived;

        public StreamMessageLogger(Action<string> notifyMessageSent, Action<string> notifyMessageReceived)
        {
            _NotifyMessageSent = notifyMessageSent;
            _NotifyMessageReceived = notifyMessageReceived;
        }

        /// <inheritdoc />
        public void NotifyMessageSent(string content)
        {
            _NotifyMessageSent?.Invoke(content);
        }

        /// <inheritdoc />
        public void NotifyMessageReceived(string content)
        {
            _NotifyMessageReceived?.Invoke(content);
        }
    }
}
