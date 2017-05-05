using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// Contains additional diagnostic information about the context in which
    /// a code action is run.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CodeActionContext
    {
        /// <summary>
        /// An array of diagnostics.
        /// </summary>
        [JsonProperty]
        public ICollection<Diagnostic> Diagnostics { get; set; }
    }

    /// <summary>
    /// A code lens represents a command that should be shown along with
    /// source text, like the number of references, a way to run tests, etc.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CodeLens
    {

        [JsonConstructor]
        public CodeLens()
        {
        }

        public CodeLens(Range range, EditorCommand command) : this(range, command, null)
        {
        }

        public CodeLens(Range range, EditorCommand command, JToken data)
        {
            Range = range;
            Command = command;
            Data = data;
        }

        /// <summary>
        /// The range in which this code lens is valid. Should only span a single line.
        /// </summary>
        [JsonProperty]
        public Range Range { get; set; }

        /// <summary>
        /// The command this code lens represents.
        /// </summary>
        /// <remarks>
        /// A code lens is "unresolved" when no command is associated to it. For performance
        /// reasons the creation of a code lens and resolving should be done in two stages.
        /// </remarks>
        [JsonProperty]
        public EditorCommand Command { get; set; }

        /// <summary>
        /// A data entry field that is preserved on a code lens item between
        /// a code lens and a code lens resolve request.
        /// </summary>
        [JsonProperty]
        public JToken Data { get; set; }
    }

    /// <summary>
    /// Descibe options to be used when registered for code lens events.
    /// </summary>
    public class CodeLensRegistrationOptions : TextDocumentRegistrationOptions
    {

        [JsonConstructor]
        public CodeLensRegistrationOptions()
            : this(false, null)
        {
        }

        public CodeLensRegistrationOptions(bool resolveProvider)
            : this(resolveProvider, null)
        {
        }

        public CodeLensRegistrationOptions(bool resolveProvider, IEnumerable<DocumentFilter> documentSelector)
            : base(documentSelector)
        {
            ResolveProvider = resolveProvider;
        }

        /// <summary>
        /// Code lens has a resolve provider as well.
        /// </summary>
        [JsonProperty]
        public bool ResolveProvider { get; set; }
    }
}
