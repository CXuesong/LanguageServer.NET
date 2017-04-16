using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VSCode.JsonRpc
{
    internal class StreamMessageWriter : IMessageWriter
    {
        internal StreamMessageWriter(Stream stream)
        {
            BaseStream = stream;
            Encoding = Encoding.UTF8;
        }

        internal StreamMessageWriter(Stream stream, Encoding encoding)
            : this(stream)
        {
            Encoding = encoding;
        }

        internal Stream BaseStream { get; private set; }
        internal Encoding Encoding { get; private set; }

        public async Task WriteAsync(IMessage message)
        {
            string json = MessageSerializer.Serialize(message);

            StringBuilder builder = new StringBuilder($"Content-Length: {json.Length}\r\nContent-Type: application/vscode-jsonrpc; charset=utf8\r\n\r\n");
            builder.Append(json);

            byte[] data = Encoding.GetBytes(builder.ToString());

            await BaseStream.WriteAsync(data, 0, data.Length);
        }
    }
}
