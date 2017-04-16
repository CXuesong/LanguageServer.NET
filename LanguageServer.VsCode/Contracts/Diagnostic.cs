namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// A value assigned to a <see cref="Diagnostic" /> determining its severity.
    /// </summary>
    public enum DiagnosticSeverity
    {
        /// <summary>
        /// Reports an error.
        /// </summary>
        Error = 1,

        /// <summary>
        /// Reports a warning.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Reports an information.
        /// </summary>
        Information = 3,

        /// <summary>
        /// Reports a hint.
        /// </summary>
        Hint = 4
    }

    /// <summary>
    /// Represents a diagnostic, such as a compiler error or warning.
    /// </summary>
    public class Diagnostic
    {
        /// <summary>
        /// The diagnostic's code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The diagnostic's message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The range to which the message applies.
        /// </summary>
        public Range Range { get; set; }

        /// <summary>
        /// The diagnostic's severity.
        /// </summary>
        public DiagnosticSeverity Severity { get; set; }

        /// <summary>
        /// A human-readable string describing the source of this diagnostic, e.g. 'typescript' or 'super lint'.
        /// </summary>
        public string Source { get; set; }
    }
}
