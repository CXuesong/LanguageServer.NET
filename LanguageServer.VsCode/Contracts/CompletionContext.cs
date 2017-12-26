using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{

    /// <summary>
    /// Contains additional information about the context in which a completion request is triggered.
    /// </summary>
    /// <remarks>
    /// When used to implement <c>textDocument/completion</c>, this instance
    /// is only available if the client specifies to send this using
    /// <c>ClientCapabilities.TextDocument.Completion.ContextSupport == true</c>.
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class CompletionContext
    {

        /// <summary>
        /// Initializes a new <see cref="CompletionContext"/>
        /// with <see cref="TriggerKind"/> set to <see cref="CompletionTriggerKind.Invoked"/>.
        /// </summary>
        [JsonConstructor]
        public CompletionContext()
        {
            TriggerKind = CompletionTriggerKind.Invoked;
        }

        /// <summary>
        /// Initializes a new <see cref="CompletionContext"/>
        /// with <see cref="TriggerKind"/> set to <see cref="CompletionTriggerKind.TriggerCharacter"/>.
        /// </summary>
        /// <param name="triggerCharacter">The character that triggers the completion.</param>
        public CompletionContext(char triggerCharacter)
        {
            TriggerKind = CompletionTriggerKind.TriggerCharacter;
            TriggerCharacter = triggerCharacter;
        }

        /// <summary>How the completion was triggered.</summary>
        [JsonProperty]
        public CompletionTriggerKind TriggerKind { get; set; }

        /// <summary>
        /// The trigger character (a single character) that has trigger code complete.
        /// Is undefined if <see cref="TriggerKind"/> is not <see cref="CompletionTriggerKind.TriggerCharacter"/>.
        /// </summary>
        [JsonProperty]
        public char TriggerCharacter { get; set; }

    }

    /// <summary>How a completion was triggered.</summary>
    public enum CompletionTriggerKind
    {
        /// <summary>
        /// Completion was triggered by typing an identifier (24x7 code complete),
        /// manual invocation (e.g Ctrl+Space) or via API.
        /// </summary>
        Invoked = 1,
        /// <summary>
        /// Completion was triggered by a trigger character specified by
        /// the <see cref="CompletionRegistrationOptions.TriggerCharacters"/> property
        /// of the <see cref="CompletionRegistrationOptions"/>.
        /// </summary>
        TriggerCharacter = 2,
    }
}
