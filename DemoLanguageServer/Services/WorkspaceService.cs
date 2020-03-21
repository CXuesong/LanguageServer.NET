using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using JsonRpc.Contracts;
using LanguageServer.VsCode.Contracts;
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
            foreach (var doc in Session.Documents.Values)
            {
                var diag = Session.DiagnosticProvider.LintDocument(doc.Document, Session.Settings.MaxNumberOfProblems);
                await Client.Document.PublishDiagnostics(doc.Document.Uri, diag);
            }
        }

        [JsonRpcMethod(IsNotification = true)]
        public async Task DidChangeWatchedFiles(ICollection<FileEvent> changes)
        {
            foreach (var change in changes)
            {
                if (!change.Uri.IsFile) continue;
                var localPath = change.Uri.AbsolutePath;
                if (string.Equals(Path.GetExtension(localPath), ".demo"))
                {
                    // If the file has been removed, we will clear the lint result about it.
                    // Note that pass null to PublishDiagnostics may mess up the client.
                    if (change.Type == FileChangeType.Deleted)
                    {
                        await Client.Document.PublishDiagnostics(change.Uri, new Diagnostic[0]);
                    }
                }
            }
        }
    }
}
