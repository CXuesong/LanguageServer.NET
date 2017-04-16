using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageServer.VsCode.JsonRpc
{
    public static class Constants
    {
        public static class JsonRpc
        {
            public const string SupportedVersion = "2.0";

            public static class Properties
            {
                public const string Error = "error";
                public const string Id = "id";
                public const string JsonRpc = "jsonrpc";
                public const string Method = "method";
                public const string Params = "params";
                public const string Result = "result";
            }
        }
    }
}
