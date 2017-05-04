using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// An event describing a change to a text document.
    /// If range and rangeLength are omitted, the new text is considered to be the full content of the document.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TextDocumentContentChangeEvent
    {

        [JsonProperty("range")] private Range? _Range;

        [JsonProperty("rangeLength")] private int? _RangeLength;

        /// <summary>
        /// The range of the document that changed.
        /// </summary>
        public Range Range
        {
            get => _Range.Value;
            set => _Range = value;
        }

        /// <summary>
        /// The length of the range (text length) that got replaced.
        /// </summary>
        public int RangeLength
        {
            get => _RangeLength.Value;
            set => _RangeLength = value;
        }

        /// <summary>
        /// Gets/sets whether both <see cref="Range"/> and <see cref="RangeLength"/> are available.
        /// </summary>
        public bool HasRange
        {
            get => _Range != null && _RangeLength != null;
            set
            {
                if (value)
                {
                    if (_Range == null) _Range = default(Range);
                    if (_RangeLength == null) _RangeLength = 0;
                }
            }
        }

        /// <summary>
        /// The new text of the range/document.
        /// </summary>
        /// <remarks>
        /// If <see cref="Range"/> and <see cref="RangeLength"/> are omitted,
        /// the new text is considered to be the full content of the document.
        /// </remarks>
        [JsonProperty]
        public string Text { get; set; }
    }

    /// <summary>
    /// Represents reasons why a text document is saved. (In <c>textDocument/willSave</c>.)
    /// </summary>
    public enum TextDocumentSaveReason
    {
        /// <summary>
        /// Manually triggered, e.g. by the user pressing save, by starting debugging,
        /// or by an API call.
        /// </summary>
        Manual = 1,
        /// <summary>
        /// Automatic after a delay.
        /// </summary>
        AfterDelay = 2,
        /// <summary>
        /// When the editor lost focus.
        /// </summary>
        FocusOut = 3,
    }
}
