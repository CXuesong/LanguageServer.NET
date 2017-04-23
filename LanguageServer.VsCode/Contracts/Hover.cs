
namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// Represents the result of a hover request - a formatted tooltip.
    /// </summary>
    public class Hover
    {
        /// <summary>
        /// A Markdown string to display in the Hover.
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// The range to which this Hover applies.
        /// </summary>
        public Range Range { get; set; }
    }
}
