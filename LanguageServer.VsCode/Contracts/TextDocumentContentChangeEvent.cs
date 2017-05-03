using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// An event describing a change to a text document.
    /// If range and rangeLength are omitted, the new text is considered to be the full content of the document.
    /// </summary>
    public class TextDocumentContentChangeEvent
    {
        /// <summary>
        /// The range of the document that changed.
        /// </summary>
        public Range? Range { get; set; }

        /// <summary>
        /// The length of the range that got replaced.
        /// </summary>
        public int? RangeLength { get; set; }

        /// <summary>
        /// The new text of the range/document.
        /// </summary>
        /// <remarks>
        /// If <see cref="Range"/> and <see cref="RangeLength"/> are omitted,
        /// the new text is considered to be the full content of the document.
        /// </remarks>
        public string Text { get; set; }
    }
}
