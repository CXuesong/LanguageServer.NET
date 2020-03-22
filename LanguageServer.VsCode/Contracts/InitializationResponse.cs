using System;
using System.Collections.Generic;
using System.Text;
using JsonRpc;
using JsonRpc.Messages;
using LanguageServer.VsCode.Contracts.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.Contracts
{

    /// <summary>
    /// The response for <c>initialize</c> request.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InitializeResult
    {
        public InitializeResult() : this(null)
        {
        }

        public InitializeResult(ServerCapabilities capabilities)
        {
            Capabilities = capabilities;
        }

        /// <summary>
        /// The capabilities the language server provides.
        /// </summary>
        [JsonProperty]
        public ServerCapabilities Capabilities { get; set; }
    }

    /// <summary>
    /// The error response for <c>initialize</c> request. (Set this object to <see cref="ResponseError.Data"/>.)
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InitializeError
    {
        /// <summary>
        /// Indicates whether the client execute the retry logic as described in the "remarks" section.
        /// </summary>
        /// <remarks>
        /// The retry logic is as follows:
        /// <list type="number">
        /// <item><description>show the message provided by the ResponseError to the user</description></item>
        /// <item><description>user selects retry or cancel</description></item>
        /// <item><description>if user selected retry the initialize method is sent again.</description></item>
        /// </list>
        /// </remarks>
        [JsonProperty]
        public bool Retry { get; set; }
    }
    
    /// <summary>
    /// Defines capabilities provided by the language server.
    /// </summary>
    [JsonObject]
    public class ServerCapabilities
    {
        /// <summary>
        /// Defines how text documents are synced. Is either a detailed structure defining each notification or
        /// for backwards compatibility the <see cref="TextDocumentSyncKind" /> number.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TextDocumentSyncOptions TextDocumentSync { get; set; }

        /// <summary>
        /// The server provides hover support.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HoverOptions HoverProvider { get; set; }

        /// <summary>
        /// The server provides completion support.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CompletionOptions CompletionProvider { get; set; }

        /// <summary>
        /// The server provides signature help support.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SignatureHelpOptions SignatureHelpProvider { get; set; }

        /// <summary>
        /// The server provides go to declaration support. (LSP 3.14)
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DeclarationOptions DeclarationProvider { get; set; }

        /// <summary>
        /// The server provides goto definition support.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DefinitionOptions DefinitionProvider { get; set; }

        /// <summary>
        /// The server provides goto type definition support. (LSP 3.6)
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public TypeDefinitionOptions TypeDefinitionProvider { get; set; }

        /// <summary>
        /// The server provides goto implementation support. (LSP 3.6)
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ImplementationOptions ImplementationProvider { get; set; }

        /// <summary>
        /// The server provides find references support.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ReferenceOptions ReferencesProvider { get; set; }

        /// <summary>
        /// The server provides document highlight support.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DocumentHighlightOptions DocumentHighlightProvider { get; set; }

        /// <summary>
        /// The server provides document symbol support.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DocumentSymbolOptions DocumentSymbolProvider { get; set; }

        /// <summary>
        /// The server provides workspace symbol support.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public WorkspaceSymbolOptions WorkspaceSymbolProvider { get; set; }

        /// <summary>
        /// The server provides code actions.
        /// </summary>
        /// <remarks>
        /// The <see cref="CodeActionOptions"/> return type is only
        /// valid if the client signals code action literal support via the property
        /// <c>textDocument.codeAction.codeActionLiteralSupport</c>.
        /// </remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CodeActionOptions CodeActionProvider { get; set; }

        /// <summary>
        /// The server provides code lens.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CodeLensOptions CodeLensProvider { get; set; }

        /// <summary>
        /// The server provides document link support.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DocumentLinkOptions DocumentLinkProvider { get; set; }

        /// <summary>
        /// The server provides color provider support. (LSP 3.6)
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DocumentColorOptions ColorProvider { get; set; }

        /// <summary>
        /// The server provides document formatting.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DocumentFormattingOptions DocumentFormattingProvider { get; set; }

        /// <summary>
        /// The server provides document range formatting.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DocumentRangeFormattingOptions DocumentRangeFormattingProvider { get; set; }

        /// <summary>
        /// The server provides document formatting on typing.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DocumentOnTypeFormattingOptions DocumentOnTypeFormattingProvider { get; set; }

        /// <summary>
        /// The server provides rename support.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public RenameOptions RenameProvider { get; set; }

        /// <summary>
        /// The server provides folding provider support. (LSP 3.10)
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public FoldingRangeOptions FoldingRangeProvider { get; set; }

        /// <summary>
        /// The server provides execute command support.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ExecuteCommandOptions ExecuteCommandProvider { get; set; }

        /// <summary>
        /// The server provides selection range support. (LSP 3.15)
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SelectionRangeOptions SelectionRangeProvider { get; set; }

        /// <summary>
        /// Workspace specific server capabilities.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public WorkspaceOptions Workspace { get; set; }

        /// <summary>
        /// Experimental server capabilities.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public JToken Experimental { get; set; }
    }

    /// <summary>
    /// Options to signal work done progress support in server capabilities. (LSP 3.15)
    /// </summary>
    public interface IWorkDoneProgressOptions
    {

        /// <summary>
        /// Signals work done progress support in server capabilities. (LSP 3.15)
        /// </summary>
        bool WorkDoneProgress { get; set; }

    }

    /// <summary>
    /// Signature help options.
    /// </summary>
    [JsonObject]
    public class SignatureHelpOptions : IWorkDoneProgressOptions
    {
        public SignatureHelpOptions()
        {

        }

        public SignatureHelpOptions(IEnumerable<char> triggerCharacters)
        {
            TriggerCharacters = triggerCharacters;
        }

        /// <summary>
        /// The characters that trigger signature help automatically.
        /// </summary>
        public IEnumerable<char> TriggerCharacters { get; set; }

        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    [JsonObject]
    public class DocumentSymbolOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    [JsonObject]
    public class CodeActionOptions : IWorkDoneProgressOptions
    {

        /// <summary>
        /// <see cref="CodeActionKind"/>s that this server may return.
        /// </summary>
        /// <remarks>
        /// The list of kinds may be generic, such as <see cref="CodeActionKind.Refactor"/>, or the server
        /// may list out every specific kind they provide.
        /// </remarks>
        public ICollection<string> CodeActionKinds { get; set; }

        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    /// <summary>
    /// Code Lens options.
    /// </summary>
    [JsonObject]
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
        public bool ResolveProvider { get; set; }
    }

    [JsonObject]
    public class WorkspaceSymbolOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
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

    [JsonObject]
    public class DocumentColorOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    [JsonObject]
    public class DocumentFormattingOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    [JsonObject]
    public class DocumentRangeFormattingOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }


    [JsonObject]
    public class RenameOptions : IWorkDoneProgressOptions
    {
        /// <summary>
        /// Renames should be checked and tested before being executed.
        /// </summary>
        public bool PrepareProvider { get; set; }

        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    [JsonObject]
    public class FoldingRangeOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    /// <summary>
    /// Signature help options.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ExecuteCommandOptions : IWorkDoneProgressOptions
    {
        public ExecuteCommandOptions()
        {
        }

        public ExecuteCommandOptions(IEnumerable<string> commands)
        {
            Commands = commands;
        }

        /// <summary>
        /// Document links have a resolve provider as well.
        /// </summary>
        [JsonProperty]
        public IEnumerable<string> Commands { get; set; }

        /// <inheritdoc />
        [JsonProperty]
        public bool WorkDoneProgress { get; set; }
    }

    [JsonObject]
    public class SelectionRangeOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    /// <summary>
    /// Defines how the host (editor) should sync document changes to the language server.
    /// </summary>
    public enum TextDocumentSyncKind
    {
        /// <summary>
        /// Documents should not be synced at all.
        /// </summary>
        None = 0,
        /// <summary>
        /// Documents are synced by always sending the full content
        /// of the document.
        /// </summary>
        Full = 1,
        /// <summary>
        /// Documents are synced by sending the full content on open.
        /// After that only incremental updates to the document are send.
        /// </summary>
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
        /// Change notifications are sent to the server. See TextDocumentSyncKind.None, TextDocumentSyncKind.Full
        /// and TextDocumentSyncKind.Incremental.
        /// </summary>
        [JsonProperty]
        public TextDocumentSyncKind Change { get; set; }

        /// <summary>
        /// Will save notifications are sent to the server.
        /// </summary>
        [JsonProperty]
        public bool WillSave { get; set; }

        /// <summary>
        /// Will save wait until requests are sent to the server.
        /// </summary>
        [JsonProperty]
        public bool WillSaveWaitUntil { get; set; }

        /// <summary>
        /// Save notifications are sent to the server.
        /// </summary>
        [JsonProperty]
        public SaveOptions Save { get; set; }
    }

    [JsonObject]
    public class HoverOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    /// <summary>
    /// Save options.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SaveOptions
    {
        [JsonConstructor]
        public SaveOptions()
        {

        }

        public SaveOptions(bool includeText)
        {
            IncludeText = includeText;
        }

        /// <summary>
        /// The client is supposed to include the content on save.
        /// </summary>
        [JsonProperty]
        public bool IncludeText { get; set; }
    }

    /// <summary>
    /// Completion options.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CompletionOptions : IWorkDoneProgressOptions
    {

        [JsonConstructor]
        public CompletionOptions()
        {

        }

        public CompletionOptions(bool resolveProvider) : this(resolveProvider, null)
        {
        }

        public CompletionOptions(bool resolveProvider, IEnumerable<char> triggerCharacters)
        {
            ResolveProvider = resolveProvider;
            TriggerCharacters = triggerCharacters;
        }

        /// <summary>
        /// The server provides support to resolve additional
        /// information for a completion item. (i.e. supports <c>completionItem/resolve</c>.)
        /// </summary>
        [JsonProperty]
        public bool ResolveProvider { get; set; }

        /// <summary>
        /// The characters that trigger completion automatically.
        /// </summary>
        [JsonProperty]
        public IEnumerable<char> TriggerCharacters { get; set; }

        /// <inheritdoc />
        [JsonProperty]
        public bool WorkDoneProgress { get; set; }
    }

    [JsonObject]
    public class DeclarationOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    [JsonObject]
    public class DefinitionOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    [JsonObject]
    public class TypeDefinitionOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    [JsonObject]
    public class ImplementationOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    [JsonObject]
    public class ReferenceOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    [JsonObject]
    public class DocumentHighlightOptions : IWorkDoneProgressOptions
    {
        /// <inheritdoc />
        public bool WorkDoneProgress { get; set; }
    }

    /// <summary>
    /// Workspace specific server capabilities.
    /// </summary>
    [JsonObject]
    public class WorkspaceOptions
    {
        /// <summary>
        /// The server supports workspace folder. (LSP 3.6)
        /// </summary>
        private WorkspaceFoldersServerCapabilities WorkspaceFolders { get; set; }
    }

    /// <summary>
    /// The server supports workspace folder. (LSP 3.6)
    /// </summary>
    [JsonObject]
    public class WorkspaceFoldersServerCapabilities
    {
        /// <summary>
        /// Whether the server has support for workspace folders.
        /// </summary>
        [JsonProperty]
        public bool Supported { get; set; }

        /// <summary>
        /// (<c>changeNotifications</c>) When specified, indicates the server wants to receive workspace folder change notifications.
        /// </summary>
        /// <value>
        /// <c>null</c> to indicate the server does not want to receive workspace folder change notifications.
        /// Or a string containing an ID under which the notification is registered on the client side.
        /// The ID can be used to unregister for these events using the <c>client/unregisterCapability</c> request (<see cref="IClient.UnregisterCapability"/>).
        /// </value>
        [JsonProperty("changeNotifications")]
        public string ChangeNotificationId { get; set; }
    }

}
