using System.Threading;
using JsonRpc.Standard.Server;

namespace DemoLanguageServer
{
    public class LanguageServerSession : Session
    {
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        public CancellationToken CancellationToken => cts.Token;

        public void StopServer()
        {
            cts.Cancel();
        }
        
    }
}