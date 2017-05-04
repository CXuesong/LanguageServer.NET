using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DemoLanguageServer
{
    internal static class Utility
    {
        public static readonly JsonSerializer CamelCaseJsonSerializer = new JsonSerializer
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
