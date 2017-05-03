using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// A document filter denotes a document through properties like language, schema or pattern.
    /// </summary>
    /// <remarks>
    /// <para>Examples are a filter that applies to TypeScript files on disk or a filter the applies to
    /// JSON files with name package.json:
    /// <code>
    /// { language: 'typescript', scheme: 'file' }
    /// { language: 'json', pattern: '**/package.json' }
    /// </code>
    /// </para>
    /// <para>
    /// A <c>document selector</c> is the combination of one or many document filters.
    /// </para>
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class DocumentFilter
    {
        /// <summary>
        /// A language id, like `typescript`.
        /// </summary>
        [JsonProperty]
        public string Language { get; set; }

        /// <summary>
        /// A Uri scheme, like `file` in `file:///C:/abc.txt` or `untitled` in `untitled:`.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// A glob pattern, like `*.{ts,js}`.
        /// </summary>
        public string Pattern { get; set; }
    }
}
