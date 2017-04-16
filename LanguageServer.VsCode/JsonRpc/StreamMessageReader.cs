using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCode.JsonRpc
{
    internal class StreamMessageReader : IMessageReader
    {
        private const int _BufferSize = 1024;
        private const string _HeaderContentLength = "Content-Length";

        internal StreamMessageReader(Stream stream)
        {
            Encoding = Encoding.UTF8;
            BaseStream = stream;
        }

        internal StreamMessageReader(Stream stream, Encoding encoding)
            : this(stream)
        {
            Encoding = encoding;
        }

        internal Stream BaseStream { get; private set; }
        internal Encoding Encoding { get; private set; }

        public async Task<IMessage> ReadAsync()
        {
            IMessage message = null;
            MessageBuffer buffer = new MessageBuffer(Encoding);

            while (message == null)
            {
                byte[] chunk = new byte[_BufferSize];
                int result = await BaseStream.ReadAsync(chunk, 0, _BufferSize);             

                if (result > 0)
                {
                    buffer.Append(chunk.Take(result).ToArray());
                }

                else
                {
                    break;
                }

                if (!buffer.Valid)
                {
                    continue;
                }

                string content = buffer.TryReadContent();

                message = MessageSerializer.Deserialize(content);
                buffer = new MessageBuffer(Encoding);
            }

            return message;
        }
    }
}
