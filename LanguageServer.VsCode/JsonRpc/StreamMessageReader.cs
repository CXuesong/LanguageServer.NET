using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageServer.VsCode.JsonRpc
{
    public class StreamMessageReader : MessageReader
    {
        private const int _BufferSize = 1024;

        public StreamMessageReader(Stream stream)
        {
            Encoding = Encoding.UTF8;
            BaseStream = stream;
        }

        public StreamMessageReader(Stream stream, Encoding encoding)
            : this(stream)
        {
            Encoding = encoding;
        }

        public Stream BaseStream { get; }

        public Encoding Encoding { get; }

        public override async Task<Message> ReadAsync()
        {
            Message message = null;
            var buffer = new MessageBuffer(Encoding);

            while (message == null)
            {
                var chunk = new byte[_BufferSize];
                var result = await BaseStream.ReadAsync(chunk, 0, _BufferSize);

                if (result > 0)
                    buffer.Append(chunk.Take(result).ToArray());
                else
                    break;

                if (!buffer.Valid)
                    continue;

                var content = buffer.TryReadContent();

                message = MessageSerializer.Deserialize(content);
                buffer = new MessageBuffer(Encoding);
            }

            return message;
        }
    }
}
