using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// Descibe options to be used when registered for document link events.
    /// </summary>
    public class DocumentOnTypeFormattingRegistrationOptions : TextDocumentRegistrationOptions
    {

        [JsonConstructor]
        public DocumentOnTypeFormattingRegistrationOptions()
            : this(default(char), null, null)
        {
        }

        public DocumentOnTypeFormattingRegistrationOptions(char firstTriggerCharacter)
            : this(firstTriggerCharacter, null, null)
        {
        }

        public DocumentOnTypeFormattingRegistrationOptions(char firstTriggerCharacter,
            ICollection<char> moreTriggerCharacter)
            : this(firstTriggerCharacter, moreTriggerCharacter, null)
        {
        }

        public DocumentOnTypeFormattingRegistrationOptions(char firstTriggerCharacter,
            ICollection<char> moreTriggerCharacter, IEnumerable<DocumentFilter> documentSelector)
            : base(documentSelector)
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
}
