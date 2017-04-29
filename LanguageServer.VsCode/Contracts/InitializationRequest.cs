using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.Contracts
{

    [JsonObject(MemberSerialization.OptIn)]
    public class ClientCapability
    {
        public static readonly ClientCapability Empty = new ClientCapability();

        public static implicit operator bool(ClientCapability cap)
        {
            return cap != null;
        }
    }

    public class DynamicClientCapability : ClientCapability
    {
        /// <summary>
        /// Whether this capability supports dynamic registration.
        /// </summary>
        [JsonProperty]
        public bool DynamicRegistration { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return DynamicRegistration ? "DynamicRegistration" : "";
        }
    }

    /// <summary>
    /// Defines the capabilities provided by the client.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ClientCapabilities
    {
        /// <summary>
        /// Workspace specific client capabilities.
        /// </summary>
        [JsonProperty]
        public WorkspaceCapability Workspace { get; set; }

        /// <summary>
        /// Text document specific client capabilities.
        /// </summary>
        [JsonProperty]
        public TextDocumentCapability TextDocument { get; set; }

        /// <summary>
        /// Experimental client capabilities.
        /// </summary>
        [JsonProperty]
        public JToken Experimental { get; set; }
    }

    /// <summary>
    /// Defines capabilities the editor / tool provides on the workspace.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkspaceCapability
    {
        /// <summary>
        /// The client supports applying batch edits to the workspace by supporting
        /// the request 'workspace/applyEdit'.
        /// </summary>
        [JsonProperty]
        public bool ApplyEdit { get; set; }

        /// <summary>
        /// Capabilities specific to <see cref="WorkspaceEdit"/>s.
        /// </summary>
        [JsonProperty]
        public WorkspaceEditCapability WorkspaceEdit { get; set; }

        /// <summary>
        /// specific to the `workspace/didChangeConfiguration` notification.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability DidChangeConfiguration { get; set; }

        /// <summary>
        /// Capabilities specific to the `workspace/didChangeWatchedFiles` notification.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability DidChangeWatchedFiles { get; set; }

        /// <summary>
        /// Capabilities specific to the `workspace/symbol` request.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability Symbol { get; set; }

        /// <summary>
        /// Capabilities specific to the `workspace/executeCommand` request.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability ExecuteCommand { get; set; }

    }

    /// <summary>
    /// Capabilities specific to <see cref="WorkspaceEdit"/>s.
    /// </summary>
    public class WorkspaceEditCapability : ClientCapability
    {
        /// <summary>
        /// The client supports versioned document changes in <see cref="WorkspaceEdit"/>s.
        /// </summary>
        [JsonProperty]
        public bool DocumentChanges { get; set; }
    }

    /// <summary>
    /// Defines capabilities the editor / tool provides on text documents.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TextDocumentCapability
    {

        /// <summary>
        /// Synchronization supports.
        /// </summary>
        [JsonProperty]
        public TextDocumentSynchronizationCapability Synchronization { get; set; }

        /// <summary>
        /// Capabilities specific to the `textDocument/completion`
        /// </summary>
        [JsonProperty]
        public TextDocumentCompletionCapability Completion { get; set; }

        /// <summary>
        /// The server provides hover support.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability Hover { get; set; }

        /// <summary>
        /// The server provides signature help support.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability SignatureHelp { get; set; }

        /// <summary>
        /// The server provides goto definition support.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability Definition { get; set; }

        /// <summary>
        /// The server provides find references support.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability References { get; set; }

        /// <summary>
        /// The server provides document highlight support.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability DocumentHighlight { get; set; }

        /// <summary>
        /// The server provides document symbol support.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability DocumentSymbol { get; set; }

        /// <summary>
        /// The server provides workspace symbol support.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability WorkspaceSymbol { get; set; }

        /// <summary>
        /// The server provides code actions.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability CodeAction { get; set; }

        /// <summary>
        /// The server provides code lens.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability CodeLens { get; set; }

        /// <summary>
        /// The server provides document formatting.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability Formatting { get; set; }

        /// <summary>
        /// The server provides document range formatting.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability RangeFormatting { get; set; }

        /// <summary>
        /// The server provides document formatting on typing.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability OnTypeFormatting { get; set; }

        /// <summary>
        /// The server provides rename support.
        /// </summary>
        [JsonProperty]
        public DynamicClientCapability Rename { get; set; }

    }

    public class TextDocumentSynchronizationCapability : DynamicClientCapability
    {
        /// <summary>
        /// The client supports sending will save notifications.
        /// </summary>
        [JsonProperty]
        public bool WillSave { get; set; }

        /// <summary>
        /// The client supports sending a will save request and
        /// waits for a response providing text edits which will
        /// be applied to the document before it is saved.
        /// </summary>
        [JsonProperty]
        public bool WillSaveWaitUntil { get; set; }

        /// <summary>
        /// The client supports did save notifications.
        /// </summary>
        [JsonProperty]
        public bool DidSave { get; set; }
    }

    public class TextDocumentCompletionCapability : DynamicClientCapability
    {
        [JsonProperty]
        public TextDocumentCompletionCapabilityItem CompletionItem { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class TextDocumentCompletionCapabilityItem
    {
        /// <summary>
        /// Client supports snippets as insert text.
        /// </summary>
        /// <remarks>
        /// A snippet can define tab stops and placeholders with `$1`, `$2`
        /// and `${3:foo}`. `$0` defines the final tab stop, it defaults to
        /// the end of the snippet. Placeholders with equal identifiers are linked,
        /// that is typing in one will update others too.
        /// </remarks>
        [JsonProperty]
        public bool SnippetSupport { get; set; }
    }

    /// <summary>
    /// Defines capabilities provided by the language server.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ServerCapabilities
    {
        /// <summary>
        /// Defines how text documents are synced. Is either a detailed structure defining each notification or
        /// for backwards compatibility the TextDocumentSyncKind number.
        /// </summary>
        public TextDocumentSyncOptions TextDocumentSync { get; set; }

        /// <summary>
        /// The server provides hover support.
        /// </summary>
        public bool HoverProvider { get; set; }

        /// <summary>
        /// The server provides completion support.
        /// </summary>
        public bool CompletionProvider { get; set; }

        /// <summary>
        /// The server provides signature help support.
        /// </summary>
        public SignatureHelpOptions SignatureHelpProvider { get; set; }

        /// <summary>
        /// The server provides goto definition support.
        /// </summary>
        public bool DefinitionProvider { get; set; }

        /// <summary>
        /// The server provides find references support.
        /// </summary>
        public bool ReferencesProvider { get; set; }

        /// <summary>
        /// The server provides document highlight support.
        /// </summary>
        public bool DocumentHighlightProvider { get; set; }

        /// <summary>
        /// The server provides document symbol support.
        /// </summary>
        public bool DocumentSymbolProvider { get; set; }

        /// <summary>
        /// The server provides workspace symbol support.
        /// </summary>
        public bool WorkspaceSymbolProvider { get; set; }

        /// <summary>
        /// The server provides code actions.
        /// </summary>
        public bool CodeActionProvider { get; set; }

        /// <summary>
        /// The server provides code lens.
        /// </summary>
        public CodeLensOptions CodeLensProvider { get; set; }

        /// <summary>
        /// The server provides document formatting.
        /// </summary>
        public bool DocumentFormattingProvider { get; set; }

        /// <summary>
        /// The server provides document range formatting.
        /// </summary>
        public bool DocumentRangeFormattingProvider { get; set; }

        /// <summary>
        /// The server provides document formatting on typing.
        /// </summary>
        public DocumentOnTypeFormattingOptions DocumentOnTypeFormattingProvider { get; set; }

        /// <summary>
        /// The server provides rename support.
        /// </summary>
        public bool RenameProvider { get; set; }

        /// <summary>
        /// The server provides document link support.
        /// </summary>
        public DocumentLinkOptions DocumentLinkProvider { get; set; }

        /// <summary>
        /// The server provides execute command support.
        /// </summary>
        public ExecuteCommandOptions ExecuteCommandProvider { get; set; }

        /// <summary>
        /// Experimental server capabilities.
        /// </summary>
        public JToken Experimental { get; set; }
    }

}
