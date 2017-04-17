using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageServer.VsCode.JsonRpc
{
    /// <summary>
    /// Indicates an error in JSON RPC parsing.
    /// </summary>
    public class JsonRpcException : Exception
    {
        public JsonRpcException(string message) : base(message)
        {

        }

        public JsonRpcException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}
