using System.Threading.Tasks;

namespace VSCode.JsonRpc
{
    internal interface IMessageReader
    {
        Task<IMessage> ReadAsync();
    }
}
