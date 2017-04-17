using System;
using System.Collections.Generic;
using System.Text;
using LanguageServer.VsCode.JsonRpc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.Server
{

    public enum TraceLevel
    {
        Off = 0,
        Messages,
        Verbose
    }

    public class InitializingEventArgs : EventArgs
    {
        public InitializingEventArgs(int processId, Uri rootUri, JToken initializationOptions,
            ClientCapabilities capabilities)
        {
            ProcessId = processId;
            RootUri = rootUri;
            InitializationOptions = initializationOptions;
            Capabilities = capabilities;
        }

        /// <summary>
        /// The process Id of the parent process that started the server.
        /// </summary>
        public int ProcessId { get; }

        /// <summary>
        /// The root path of the workspace. Is null if no folder is open.
        /// </summary>
        public Uri RootUri { get; }

        /// <summary>
        /// User provided initialization options.
        /// </summary>
        public JToken InitializationOptions { get; }

        /// <summary>
        /// The capabilities provided by the client (editor).
        /// </summary>
        public ClientCapabilities Capabilities { get; }

    }

    /// <summary>
    /// Defines the capabilities provided by the client.
    /// </summary>
    public class ClientCapabilities
    {
        /// <summary>
        /// Workspace specific client capabilities.
        /// </summary>
        public WorkspaceClientCapabilites Workspace { get; }

        /// <summary>
        /// Text document specific client capabilities.
        /// </summary>
        public TextDocumentClientCapabilities TextDocument { get; }

        /// <summary>
        /// Experimental client capabilities.
        /// </summary>
        public JToken Experimental { get; }

        public ClientCapabilities(WorkspaceClientCapabilites workspace, TextDocumentClientCapabilities textDocument,
            JToken experimental)
        {
            Workspace = workspace ?? WorkspaceClientCapabilites.Empty;
            TextDocument = textDocument ?? TextDocumentClientCapabilities.Empty;
            Experimental = experimental;
        }
    }

    /// <summary>
    /// Defines capabilities the editor / tool provides on the workspace.
    /// </summary>
    [JsonConverter(typeof(ConstructorInjectedJsonConverter<WorkspaceClientCapabilites>))]
    public class WorkspaceClientCapabilites
    {
        /// <summary>
        /// Gets an empty capability set.
        /// </summary>
        public static readonly WorkspaceClientCapabilites Empty = new WorkspaceClientCapabilites(null);

        /// <summary>
        /// The client supports applying batch edits to the workspace by supporting
        /// the request 'workspace/applyEdit'.
        /// </summary>
        public bool SupportsApplyEdit { get; }

        /// <summary>
        /// The client supports versioned document changes in `WorkspaceEdit`s.
        /// </summary>
        public bool SupportsVersionedDocumentChanges { get; }

        // TODO other properties.

        public WorkspaceClientCapabilites(JToken jroot)
        {
            SupportsApplyEdit = (bool?) jroot?["applyEdit"] ?? false;
            SupportsVersionedDocumentChanges = (bool?) jroot?["workspaceEdit"]?["documentChanges"] ?? false;
        }
    }

    /// <summary>
    /// Defines capabilities the editor / tool provides on text documents.
    /// </summary>
    [JsonConverter(typeof(ConstructorInjectedJsonConverter<TextDocumentClientCapabilities>))]
    public class TextDocumentClientCapabilities
    {
        /// <summary>
        /// Gets an empty capability set.
        /// </summary>
        public static readonly TextDocumentClientCapabilities Empty = new TextDocumentClientCapabilities(null);

        /// <summary>
        /// The client supports sending will save notifications.
        /// </summary>
        public bool SavingEvent { get; }

        /// <summary>
        /// The client supports sending a will save request and
        /// waits for a response providing text edits which will
        /// be applied to the document before it is saved.
        /// </summary>
        public bool WillSaveUntilEvent { get; }

        /// <summary>
        /// The client supports did save notifications.
        /// </summary>
        public bool SavedEvent { get; }

        /// <summary>
        /// Client supports snippets as insert text.
        /// </summary>
        /// <remarks>
        /// A snippet can define tab stops and placeholders with `$1`, `$2`
        /// and `${3:foo}`. `$0` defines the final tab stop, it defaults to
        /// the end of the snippet. Placeholders with equal identifiers are linked,
        /// that is typing in one will update others too.
        /// </remarks>
        public bool Snippet { get; }

        internal TextDocumentClientCapabilities(JToken jroot)
        {
            SavingEvent = (bool?) jroot?["synchronization"]?["willSave"] ?? false;
            WillSaveUntilEvent = (bool?) jroot?["synchronization"]?["willSaveWaitUntil"] ?? false;
            SavedEvent = (bool?) jroot?["synchronization"]?["didSave"] ?? false;
            Snippet = (bool?) jroot?["completion"]?["completionItem"]?["snippetSupport"] ?? false;
        }
    }

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

    /// <summary>
    /// Signature help options.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SignatureHelpOptions
    {
        public SignatureHelpOptions()
        {

        }

        public SignatureHelpOptions(ICollection<string> triggerCharacters)
        {
            TriggerCharacters = triggerCharacters;
        }

        /// <summary>
        /// The characters that trigger signature help automatically.
        /// </summary>
        [JsonProperty]
        public ICollection<string> TriggerCharacters { get; set; }
    }

    /// <summary>
    /// Code Lens options.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CodeLensOptions
    {
        public CodeLensOptions()
        {

        }

        public CodeLensOptions(bool resolveProvider)
        {
            ResolveProvider = resolveProvider;
        }

        /// <summary>
        /// Code lens has a resolve provider as well.
        /// </summary>
        [JsonProperty]
        public bool ResolveProvider { get; set; }
    }

    /// <summary>
    /// Format document on type options.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DocumentOnTypeFormattingOptions
    {
        public DocumentOnTypeFormattingOptions()
        {

        }

        public DocumentOnTypeFormattingOptions(char firstTriggerCharacter, ICollection<char> moreTriggerCharacter)
        {
            FirstTriggerCharacter = firstTriggerCharacter;
            MoreTriggerCharacter = moreTriggerCharacter;
        }

        /// <summary>
        /// A character on which formatting should be triggered.
        /// </summary>
        [JsonProperty]
        public char FirstTriggerCharacter { get; set; }

        /// <summary>
        /// More trigger characters.
        /// </summary>
        [JsonProperty]
        public ICollection<char> MoreTriggerCharacter { get; set; }
    }

    /// <summary>
    /// Signature help options.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DocumentLinkOptions
    {
        public DocumentLinkOptions()
        {

        }

        public DocumentLinkOptions(bool resolveProvider)
        {
            ResolveProvider = resolveProvider;
        }

        /// <summary>
        /// Document links have a resolve provider as well.
        /// </summary>
        [JsonProperty]
        public bool ResolveProvider { get; set; }
    }

    /// <summary>
    /// Signature help options.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ExecuteCommandOptions
    {
        public ExecuteCommandOptions()
        {

        }

        public ExecuteCommandOptions(IList<string> commands)
        {
            Commands = commands;
        }

        /// <summary>
        /// Document links have a resolve provider as well.
        /// </summary>
        [JsonProperty]
        public IList<string> Commands { get; set; }
    }

    /// <summary>
    /// Defines how the host (editor) should sync document changes to the language server.
    /// </summary>
    public enum TextDocumentSyncKind
    {
        None = 0,
        Full = 1,
        Incremental = 2
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class TextDocumentSyncOptions
    {
        /// <summary>
        /// Open and close notifications are sent to the server.
        /// </summary>
        [JsonProperty]
        public bool OpenClose { get; set; }

        /// <summary>
        /// Change notificatins are sent to the server. See TextDocumentSyncKind.None, TextDocumentSyncKind.Full
        /// and TextDocumentSyncKind.Incremental.
        /// </summary>
        [JsonProperty("change")]
        public TextDocumentSyncKind ChangedEvent { get; set; }

        /// <summary>
        /// Will save notifications are sent to the server.
        /// </summary>
        [JsonProperty("willSave")]
        public bool SavingEvent { get; set; }

        /// <summary>
        /// Will save wait until requests are sent to the server.
        /// </summary>
        [JsonProperty]
        public bool WillSaveWaitUntil { get; set; }

        /// <summary>
        /// Save notifications are sent to the server.
        /// </summary>
        [JsonProperty("save")]
        private SaveEventOptions SaveEvent { get; set; }
    }

    /// <summary>
    /// Save options. (export interface SaveOptions)
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SaveEventOptions
    {
        public SaveEventOptions()
        {
            
        }

        public SaveEventOptions(bool includeText)
        {
            IncludeText = includeText;
        }

        /// <summary>
        /// The client is supposed to include the content on save.
        /// </summary>
        public bool IncludeText { get; set; }
    }
}
