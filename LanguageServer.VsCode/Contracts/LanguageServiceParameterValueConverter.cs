using System;
using System.Collections.Generic;
using System.Text;
using JsonRpc.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// A <see cref="IJsonValueConverter"/> implementation that is supposed to be used with
    /// LSP over JSON-RPC.
    /// </summary>
    public class LanguageServiceParameterValueConverter : JsonValueConverter
    {

        private static readonly JsonSerializer serializer = new JsonSerializer
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            // That is, by default `null` in the received JSON will be treated as if it does not exist.
            NullValueHandling = NullValueHandling.Ignore
        };

        public LanguageServiceParameterValueConverter() : base(serializer)
        {
        }

    }
}
