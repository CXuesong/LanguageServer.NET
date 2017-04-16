using System.Threading.Tasks;

namespace VSCode.JsonRpc
{
    internal interface IMessageWriter
    {
        Task WriteAsync(IMessage message);
    }
}
