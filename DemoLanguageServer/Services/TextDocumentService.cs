using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonRpc.Standard.Contracts;
using JsonRpc.Standard.Server;
using LanguageServer.VsCode.Contracts;

namespace DemoLanguageServer.Services
{
    public class TextDocumentService : JsonRpcService
    {
        [JsonRpcMethod("textDocument/hover")]
        public async Task<Hover> Hover(TextDocumentIdentifier textDocument, Position position)
        {
            await Task.Delay(1000);
            return new Hover {Contents = "Test _hover_ @" + position + "\n\n" + textDocument};
        }
    }
}
