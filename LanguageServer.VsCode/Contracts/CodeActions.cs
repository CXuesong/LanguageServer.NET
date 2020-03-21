using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageServer.VsCode.Contracts
{

    /// <summary>
    /// The kind of a code action. Kinds are a hierarchical list of identifiers separated by <c>.</c>, e.g. <c>"refactor.extract.function"</c>.
    /// </summary>
    public static class CodeActionKind
    {

        /// <summary>
        /// Empty kind.
        /// </summary>
        public const string Empty = "";

        /// <summary>
        /// Base kind for quickfix actions: "quickfix".
        /// </summary>
        public const string QuickFix = "quickfix";

        /// <summary>
        /// Base kind for refactoring actions: "refactor".
        /// </summary>
        public const string Refactor = "refactor";

        /// <summary>
        /// Base kind for refactoring extraction actions: "refactor.extract".
        ///
        /// Example extract actions:
        ///
        /// - Extract method
        /// - Extract function
        /// - Extract variable
        /// - Extract interface from class
        /// - ...
        /// </summary>
        public const string RefactorExtract = "refactor.extract";

        /// <summary>
        /// Base kind for refactoring inline actions: "refactor.inline".
        ///
        /// Example inline actions:
        ///
        /// - Inline function
        /// - Inline variable
        /// - Inline constant
        /// - ...
        /// </summary>
        public const string RefactorInline = "refactor.inline";

        /// <summary>
        /// Base kind for refactoring rewrite actions: "refactor.rewrite".
        ///
        /// Example rewrite actions:
        ///
        /// - Convert JavaScript function to class
        /// - Add or remove parameter
        /// - Encapsulate field
        /// - Make method static
        /// - Move method to base class
        /// - ...
        /// </summary>
        public const string RefactorRewrite = "refactor.rewrite";

        /// <summary>
        /// Base kind for source actions: `source`.
        ///
        /// Source code actions apply to the entire file.
        /// </summary>
        public const string Source = "source";

        /// <summary>
        /// Base kind for an organize imports source action: `source.organizeImports`.
        /// </summary>
        public const string SourceOrganizeImports = "source.organizeImports";

    }

}
