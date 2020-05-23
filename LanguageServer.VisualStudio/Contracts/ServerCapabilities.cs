using System;
using System.Collections.Generic;
using System.Text;
using LanguageServer.VsCode.Contracts;
using Newtonsoft.Json;

namespace LanguageServer.VisualStudio.Contracts
{
    public class VsServerCapabilities : ServerCapabilities
    {

        /// <summary>
        /// (Visual Studio) The server provides hover support.
        /// </summary>
        /// <remarks>VS does not support object value for now.</remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public new bool HoverProvider
        {
            get => base.HoverProvider != null;
            set => base.HoverProvider = value ? (base.HoverProvider ?? new HoverOptions()) : null;
        }

    }
}
