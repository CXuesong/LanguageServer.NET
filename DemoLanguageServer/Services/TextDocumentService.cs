using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonRpc.Standard.Contracts;
using JsonRpc.Standard.Server;
using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Server;

namespace DemoLanguageServer.Services
{
    [JsonRpcScope(MethodPrefix = "textDocument/")]
    public class TextDocumentService : DemoLanguageServiceBase
    {
        [JsonRpcMethod]
        public async Task<Hover> Hover(TextDocumentIdentifier textDocument, Position position)
        {
            await Task.Delay(1000);
            return new Hover {Contents = "Test _hover_ @" + position + "\n\n" + textDocument};
        }

        [JsonRpcMethod(IsNotification = true)]
        public async Task DidOpen(TextDocumentItem textDocument)
        {
            var doc = TextDocument.Load<FullTextDocument>(textDocument);
            Documents.Add(doc);
            var diag = Session.DiagnosticProvider.LintDocument(doc, Session.Settings.MaxNumberOfProblems);
            await Client.Document.PublishDiagnostics(doc.Uri, diag);
        }

        [JsonRpcMethod(IsNotification = true)]
        public async Task DidChange(TextDocumentIdentifier textDocument,
            ICollection<TextDocumentContentChangeEvent> contentChanges)
        {
            var doc = Documents[textDocument];
            doc.ApplyChanges(contentChanges);
            //await Client.Window.LogMessage(MessageType.Log, "-----------");
            //await Client.Window.LogMessage(MessageType.Log, doc.Content);
            var diag = Session.DiagnosticProvider.LintDocument(doc, Session.Settings.MaxNumberOfProblems);
            await Client.Document.PublishDiagnostics(doc.Uri, diag);
        }

        [JsonRpcMethod(IsNotification = true)]
        public void WillSave(TextDocumentIdentifier textDocument, TextDocumentSaveReason reason)
        {
            //Client.Window.LogMessage(MessageType.Log, "-----------");
            //Client.Window.LogMessage(MessageType.Log, Documents[textDocument].Content);
        }

        [JsonRpcMethod(IsNotification = true)]
        public void DidClose(TextDocumentIdentifier textDocument)
        {
            Documents.Remove(textDocument);
        }
    }
}
