using System;
using System.Collections.Generic;
using System.Text;
using JsonRpc.Standard;
using Newtonsoft.Json;

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
        /// Change notificatins are sent to the server. See TextDocumentSyncKind.None, TextDocumentSyncKind.Full
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
        private SaveEventOptions Save { get; set; }
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
        [JsonProperty]
        public bool IncludeText { get; set; }
    }
}
