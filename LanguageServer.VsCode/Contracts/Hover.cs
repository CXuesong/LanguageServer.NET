
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// Represents the result of a hover request - a formatted tooltip.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Hover
    {
        [JsonConstructor]
        public Hover()
        {
            
        }

        public Hover(MarkupContent contents) : this(contents, new Range())
        {
        }

        public Hover(MarkupContent contents, Range range)
        {
            Contents = contents;
            Range = range;
        }

        /// <summary>
        /// A Markdown string to display in the Hover.
        /// </summary>
        [JsonProperty]
        public MarkupContent Contents { get; set; }

        /// <summary>
        /// An optional range that is a range inside a text document.
        /// that is used to visualize a hover, e.g. by changing the background color.
        /// </summary>
        [JsonProperty]
        public Range Range { get; set; }
    }
}
