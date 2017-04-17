using System.Threading;
using System.Threading.Tasks;

namespace LanguageServer.VsCode.JsonRpc
{
    public abstract class MessageWriter
    {
        public abstract void Write(Message message);
    }
}
