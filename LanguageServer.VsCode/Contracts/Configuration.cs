using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageServer.VsCode.Contracts
{

    /// <summary>
    /// A ConfigurationItem consists of the configuration section to ask for and an additional scope URI.
    /// </summary>
    public class ConfigurationItem
    {

        public ConfigurationItem(Uri scopeUri, string section)
        {
            ScopeUri = scopeUri;
            Section = section;
        }

        /// <summary>
        /// The scope to get the configuration section for.
        /// </summary>
        public Uri ScopeUri { get; }

        /// <summary>
        /// The configuration section asked for.
        /// </summary>
        public string Section { get; }

    }

}
