using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// Additional information about the context in which a signature help request was triggered. (LSP 3.15)
    /// </summary>
    [JsonObject]
    public class SignatureHelpContext
    {

        /// <summary>
        /// Action that caused signature help to be triggered.
        /// </summary>
        public SignatureHelpTriggerKind TriggerKind { get; set; }

        /// <summary>
        /// Character that caused signature help to be triggered.
        /// </summary>
        public char TriggerCharacter { get; set; }

        /// <summary>
        /// `true` if signature help was already showing when it was triggered.
        /// </summary>
        public bool IsRetrigger { get; set; }

        /// <summary>
        /// The currently active `SignatureHelp`.
        /// </summary>
        /// <remarks>
        /// The `activeSignatureHelp` has its `SignatureHelp.activeSignature` field updated based on
        /// the user navigating through available signatures.
        /// </remarks>
        public SignatureHelp ActiveSignatureHelp { get; set; }

    }

    public enum SignatureHelpTriggerKind
    {
        /// <summary>
        /// Signature help was invoked manually by the user or by a command.
        /// </summary>
        Invoked = 1,
        /// <summary>
        /// Signature help was triggered by a trigger character.
        /// </summary>
        TriggerCharacter = 2,
        /// <summary>
        /// Signature help was triggered by the cursor moving or by the document content changing.
        /// </summary>
        ContentChange = 3
    }
}
