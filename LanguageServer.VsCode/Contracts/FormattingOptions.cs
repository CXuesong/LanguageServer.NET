using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// Value-object describing what options formatting should use.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class FormattingOptions
    {
        [JsonConstructor]
        public FormattingOptions()
        {

        }

        /// <summary>
        /// Size of a tab in spaces.
        /// </summary>
        [JsonProperty]
        public int TabSize { get; set; }

        /// <summary>
        /// Prefer spaces over tabs.
        /// </summary>
        [JsonProperty]
        public bool InsertSpaces { get; set; }

        /// <summary>
        /// Signature for further properties.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> ExtensionData { get; set; }
    }
}
