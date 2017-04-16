using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.JsonRpc
{
    /// <summary>
    /// Error codes defined by the JSON-RPC 2.0 specification.
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// Internal JSON-RPC error.
        /// </summary>
        InternalError = -32603,

        /// <summary>
        /// Invalid method parameter(s).
        /// </summary>
        InvalidParams = -32602,

        /// <summary>
        /// The JSON sent is not a valid Request object.
        /// </summary>
        InvalidRequest = -32600,

        /// <summary>
        /// The method does not exist / is not available.
        /// </summary>
        MethodNotFound = -32601,

        /// <summary>
        /// Invalid JSON was received by the server. An error occurred on the server while parsing the JSON text.
        /// </summary>
        ParseError = -32700,

        /// <summary>
        /// Defined by the protocol. The request has been cancelled.
        /// </summary>
        RequestCancelled = -32800,
    }

    public interface IResponseError
    {
        ErrorCode Code { get; set; }

        string Message { get; set; }

        object Data { get; set; }
    }

    [JsonObject]
    public class ResponseError<T> : IResponseError
    {
        [JsonProperty]
        public ErrorCode Code { get; set; }

        [JsonProperty]
        public string Message { get; set; }

        [JsonProperty]
        public T Data { get; set; }

        object IResponseError.Data
        {
            get => Data;
            set => Data = (T) value;
        }
    }
}
