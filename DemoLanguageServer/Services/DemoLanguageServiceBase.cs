using System;
using System.Collections.Generic;
using System.Text;
using JsonRpc.Standard.Server;
using LanguageServer.VsCode.Contracts.Client;
using LanguageServer.VsCode.Server;

namespace DemoLanguageServer.Services
{
    public class DemoLanguageServiceBase : JsonRpcService
    {

        protected LanguageServerSession Session => (LanguageServerSession)RequestContext.Session;

        protected ClientProxy Client => Session.Client;

        protected TextDocumentCollection Documents => Session.Documents;

    }
}
