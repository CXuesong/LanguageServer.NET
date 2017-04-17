using System.IO;
using System.Text;
using System.Threading;
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

        public override void Write(Message message)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms, Encoding))
                    RpcSerializer.SerializeMessage(writer, message);
                using (var writer = new StreamWriter(BaseStream, Encoding))
                {
                    writer.Write("Content-Length: ");
                    writer.Write(ms.Length);
                    writer.Write("\r\nContent-Type: application/vscode-jsonrpc; charset=utf8\r\n\r\n");
                }
                ms.CopyTo(BaseStream);
            }
        }
    }
}
