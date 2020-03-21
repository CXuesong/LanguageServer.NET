using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonRpc.Contracts;
using LanguageServer.VsCode.Contracts;

namespace DemoLanguageServer.Services
{
    [JsonRpcScope(MethodPrefix = "completionItem/")]
    public class CompletionItemService : DemoLanguageServiceBase
    {
        // The request is sent from the client to the server to resolve additional information
        // for a given completion item.
        [JsonRpcMethod(AllowExtensionData = true)]
        public CompletionItem Resolve()
        {
            var item = RequestContext.Request.Parameters.ToObject<CompletionItem>(Utility.CamelCaseJsonSerializer);
            // Add a pair of square brackets around the inserted text.
            item.InsertText = $"[{item.Label}]";
            return item;
        }
    }
}
