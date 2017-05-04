using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// A document highlight is a range inside a text document which deserves
    /// special attention. Usually a document highlight is visualized by changing
    /// the background color of its range.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DocumentHighlight
    {
        [JsonConstructor]
        public DocumentHighlight()
        {
            
        }

        public DocumentHighlight(Range range, DocumentHighlightKind kind)
        {
            Range = range;
            Kind = kind;
        }

        /// <summary>
        /// The range this highlight applies to.
        /// </summary>
        [JsonProperty]
        public Range Range { get; set; }

        /// <summary>
        /// The highlight kind, default is <see cref="DocumentHighlightKind.Text"/>.
        /// </summary>
        [JsonProperty]
        public DocumentHighlightKind Kind { get; set; } = DocumentHighlightKind.Text;
    }

    /// <summary>
    /// Write-access of a symbol, like writing to a variable.
    /// </summary>
    public enum DocumentHighlightKind
    {
        /// <summary>
        /// A textual occurrence.
        /// </summary>
        Text = 1,
        /// <summary>
        /// Read-access of a symbol, like reading a variable.
        /// </summary>
        Read = 2,
        /// <summary>
        /// Write-access of a symbol, like writing to a variable.
        /// </summary>
        Write = 3,
    }
}
