using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// Used with <c>textDocument/references</c>.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ReferenceContext
    {
        [JsonConstructor]
        public ReferenceContext()
        {
            
        }

        public ReferenceContext(bool includeDeclaration)
        {
            IncludeDeclaration = includeDeclaration;
        }

        /// <summary>
        /// Include the declaration of the current symbol.
        /// </summary>
        [JsonProperty]
        public bool IncludeDeclaration { get; set; }
    }
}
