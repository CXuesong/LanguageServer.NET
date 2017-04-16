using System.Threading.Tasks;

namespace LanguageServer.VsCode.JsonRpc
{
    public abstract class MessageWriter
    {
        public abstract Task WriteAsync(Message message);
    }
}
