using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.JsonRpc
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GeneralRequestEventArgs : EventArgs
    {
        [JsonExtensionData]
        public IDictionary<string, JToken> ExtensionData { get; private set; }
    }
}
