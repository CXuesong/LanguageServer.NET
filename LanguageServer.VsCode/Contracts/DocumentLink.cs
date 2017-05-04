using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// A document link is a range in a text document that links to an internal or external resource, like another
    /// text document or a web site.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DocumentLink
    {
        /// <summary>
        /// The range this link applies to.
        /// </summary>
        [JsonProperty]
        public Range Range { get; set; }

        /// <summary>
        /// The range this link applies to. If missing a resolve request is sent later.
        /// </summary>
        [JsonProperty]
        public Uri Uri { get; set; }

    }

    public class DocumentLinkRegistrationOptions : TextDocumentRegistrationOptions
    {
        /// <summary>
        /// Document links have a resolve provider as well.
        /// </summary>
        [JsonProperty]
        public bool ResolveProvider { get; set; }
    }
}
