using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.JsonRpc
{
    /// <summary>
    /// Error codes, including those who are defined by the JSON-RPC 2.0 specification.
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// Internal JSON-RPC error. (JSON-RPC)
        /// </summary>
        InternalError = -32603,

        /// <summary>
        /// Invalid method parameter(s). (JSON-RPC)
        /// </summary>
        InvalidParams = -32602,

        /// <summary>
        /// The JSON sent is not a valid Request object. (JSON-RPC)
        /// </summary>
        InvalidRequest = -32600,

        /// <summary>
        /// The method does not exist / is not available. (JSON-RPC)
        /// </summary>
        MethodNotFound = -32601,

        /// <summary>
        /// Invalid JSON was received by the server. An error occurred on the server while parsing the JSON text. (JSON-RPC)
        /// </summary>
        ParseError = -32700,

        /// <summary>
        /// Defined by the protocol. The request has been cancelled. (JSON-RPC)
        /// </summary>
        RequestCancelled = -32800,

        /// <summary>
        /// There is unhandled CLR exception occurred during the process of request.
        /// </summary>
        UnhandledClrException = 1000,
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ResponseError
    {
        public ResponseError(ErrorCode code, string message) : this(code, message, null)
        {
        }

        public ResponseError(ErrorCode code, string message, object data)
        {
            Code = code;
            Message = message;
            SetData(data);
        }

        [JsonProperty]
        public ErrorCode Code { get; set; }

        [JsonProperty]
        public string Message { get; set; }

        /// <summary>
        /// A <see cref="JToken" /> representing parameters for the method.
        /// </summary>
        [JsonProperty]
        public JToken Data { get; set; }

        public object GetData(Type DataType)
        {
            return Data?.ToObject(DataType, RpcSerializer.Serializer);
        }

        public T GetData<T>()
        {
            return Data == null ? default(T) : Data.ToObject<T>(RpcSerializer.Serializer);
        }

        public void SetData(object newData)
        {
            Data = newData == null ? null : JToken.FromObject(newData, RpcSerializer.Serializer);
        }
    }
}
