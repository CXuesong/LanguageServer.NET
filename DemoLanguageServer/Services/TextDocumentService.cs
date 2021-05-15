using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JsonRpc.Contracts;
using JsonRpc.Server;
using LanguageServer.VsCode;
using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Server;

namespace DemoLanguageServer.Services
{
    [JsonRpcScope(MethodPrefix = "textDocument/")]
    public class TextDocumentService : DemoLanguageServiceBase
    {
        [JsonRpcMethod]
        public async Task<Hover> Hover(TextDocumentIdentifier textDocument, Position position, CancellationToken ct)
        {
            // Note that Hover is cancellable.
            await Task.Delay(1000, ct);
            return new Hover {Contents = "Test _hover_ @" + position + "\n\n" + textDocument};
        }

        [JsonRpcMethod]
        public SignatureHelp SignatureHelp(TextDocumentIdentifier textDocument, Position position, SignatureHelpContext context = null)
        {
            return new SignatureHelp(new List<SignatureInformation>
            {
                new SignatureInformation("**Function1**", "Documentation1"),
                new SignatureInformation("**Function2** <strong>test</strong>", "Documentation2"),
            });
        }

        [JsonRpcMethod(IsNotification = true)]
        public async Task DidOpen(TextDocumentItem textDocument)
        {
            var doc = new SessionDocument(textDocument);
            var session = Session;
            doc.DocumentChanged += async (sender, args) =>
            {
                // Lint the document when it's changed.
                var doc1 = ((SessionDocument) sender).Document;
                var diag1 = session.DiagnosticProvider.LintDocument(doc1, session.Settings.MaxNumberOfProblems);
                if (session.Documents.ContainsKey(doc1.Uri))
                {
                    // In case the document has been closed when we were linting…
                    await session.Client.Document.PublishDiagnostics(doc1.Uri, diag1);
                }
            };
            Session.Documents.TryAdd(textDocument.Uri, doc);
            var diag = Session.DiagnosticProvider.LintDocument(doc.Document, Session.Settings.MaxNumberOfProblems);
            await Client.Document.PublishDiagnostics(textDocument.Uri, diag);
        }

        [JsonRpcMethod(IsNotification = true)]
        public void DidChange(TextDocumentIdentifier textDocument,
            ICollection<TextDocumentContentChangeEvent> contentChanges)
        {
            Session.Documents[textDocument.Uri].NotifyChanges(contentChanges);
        }

        [JsonRpcMethod(IsNotification = true)]
        public void WillSave(TextDocumentIdentifier textDocument, TextDocumentSaveReason reason)
        {
            //Client.Window.LogMessage(MessageType.Log, "-----------");
            //Client.Window.LogMessage(MessageType.Log, Documents[textDocument].Content);
        }

        [JsonRpcMethod(IsNotification = true)]
        public async Task DidClose(TextDocumentIdentifier textDocument)
        {
            if (textDocument.Uri.IsUntitled())
            {
                await Client.Document.PublishDiagnostics(textDocument.Uri, new Diagnostic[0]);
            }
            Session.Documents.TryRemove(textDocument.Uri, out _);
        }

        private static readonly CompletionItem[] PredefinedCompletionItems =
        {
            new CompletionItem(".NET", CompletionItemKind.Keyword,
                "Keyword1",
                MarkupContent.Markdown("Short for **.NET Framework**, a software framework by Microsoft (possibly its subsets) or later open source .NET Core."),
                null),
            new CompletionItem(".NET Standard", CompletionItemKind.Keyword,
                "Keyword2",
                "The .NET Standard is a formal specification of .NET APIs that are intended to be available on all .NET runtimes.",
                null),
            new CompletionItem(".NET Framework", CompletionItemKind.Keyword,
                "Keyword3",
                ".NET Framework (pronounced dot net) is a software framework developed by Microsoft that runs primarily on Microsoft Windows.", null),
        };

        [JsonRpcMethod]
        public CompletionList Completion(TextDocumentIdentifier textDocument, Position position, CompletionContext context)
        {
            return new CompletionList(PredefinedCompletionItems);
        }

    }
}
