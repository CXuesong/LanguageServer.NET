using System.Threading.Tasks;

namespace LanguageServer.VsCode.JsonRpc
{
    public abstract class MessageReader
    {
        public abstract Task<Message> ReadAsync();
    }
}
