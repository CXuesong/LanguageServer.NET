using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LanguageServer.VsCode.JsonRpc
{
    public class StreamMessageWriter : MessageWriter
    {
        public StreamMessageWriter(Stream stream)
        {
            BaseStream = stream;
            Encoding = Encoding.UTF8;
        }

        public StreamMessageWriter(Stream stream, Encoding encoding)
            : this(stream)
        {
            Encoding = encoding;
        }

        public Stream BaseStream { get; }

        public Encoding Encoding { get; }

        public override async Task WriteAsync(Message message)
        {
            var json = RpcSerializer.SerializeMessage(message);

            var builder = new StringBuilder($"Content-Length: {json.Length}\r\nContent-Type: application/vscode-jsonrpc; charset=utf8\r\n\r\n");
            builder.Append(json);

            var data = Encoding.GetBytes(builder.ToString());

            await BaseStream.WriteAsync(data, 0, data.Length);
        }
    }
}
