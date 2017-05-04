using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonRpc.Standard.Contracts;
using Newtonsoft.Json.Linq;

namespace DemoLanguageServer.Services
{
    [JsonRpcScope(MethodPrefix = "workspace/")]
    public class WorkspaceService : DemoLanguageServiceBase
    {
        [JsonRpcMethod(IsNotification = true)]
        public async Task DidChangeConfiguration(SettingsRoot settings)
        {
            Session.Settings = settings.DemoLanguageServer;
            foreach (var doc in Session.Documents)
            {
                var diag = Session.DiagnosticProvider.LintDocument(doc, Session.Settings.MaxNumberOfProblems);
                await Client.Document.PublishDiagnostics(doc.Uri, diag);
            }
        }
    }
}
