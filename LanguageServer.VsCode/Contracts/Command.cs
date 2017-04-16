using System.Collections.Generic;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// Represents a reference to a VS Code command.
    /// </summary>
    public class EditorCommand
    {
        /// <summary>
        /// Arguments that the command handler should be invoked with.
        /// </summary>
        public IList<object> Arguments { get; set; }

        /// <summary>
        /// The identifier of the actual command handler.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Title of the command, like `save`.
        /// </summary>
        public string Title { get; set; }
    }
}
