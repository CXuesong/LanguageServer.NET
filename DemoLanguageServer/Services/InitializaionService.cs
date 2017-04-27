using System;
using JsonRpc.Standard.Contracts;
using JsonRpc.Standard.Server;
using LanguageServer.VsCode.Contracts;
using Newtonsoft.Json.Linq;

namespace DemoLanguageServer.Services
{
    public class InitializaionService : JsonRpcService
    {
        [JsonRpcMethod(AllowExtensionData = true)]
        public object Initialize(int processId, Uri rootUri, ClientCapabilities capabilities,
            JToken initializationOptions = null, string trace = null)
        {
            return new {Capabilities = new ServerCapabilities {HoverProvider = true}};
        }

        [JsonRpcMethod]
        public void Initialized()
        {
            
        }

        [JsonRpcMethod]
        public void Shutdown()
        {

        }

        [JsonRpcMethod]
        public void Exit()
        {
            ((MySession)RequestContext.Session).StopServer();
        }
    }
}
