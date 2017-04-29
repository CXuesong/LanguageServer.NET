using System;
using System.Threading.Tasks;
using JsonRpc.Standard.Contracts;
using JsonRpc.Standard.Server;
using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Contracts.Client;
using Newtonsoft.Json.Linq;

namespace DemoLanguageServer.Services
{
    public class InitializaionService : JsonRpcService
    {
        protected LanguageServerSession Session => (LanguageServerSession) RequestContext.Session;

        [JsonRpcMethod(AllowExtensionData = true)]
        public object Initialize(int processId, Uri rootUri, ClientCapabilities capabilities,
            JToken initializationOptions = null, string trace = null)
        {
            return new {Capabilities = new ServerCapabilities {HoverProvider = true}};
        }

        [JsonRpcMethod]
        public async Task Initialized()
        {
            await Session.ClientWindow.ShowMessage(MessageType.Info, "Hello from language server.");
            var choice = await Session.ClientWindow.ShowMessage(MessageType.Warning, "Wanna drink?", "Yes", "No");
            await Session.ClientWindow.ShowMessage(MessageType.Info, $"You chose {choice}.");
        }

        [JsonRpcMethod]
        public void Shutdown()
        {

        }

        [JsonRpcMethod]
        public void Exit()
        {
            Session.StopServer();
        }
    }
}
