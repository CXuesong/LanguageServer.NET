using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.JsonRpc
{
    /// <summary>
    /// An <see cref="Message" /> implementation representing a JSON-RPC response.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ResponseMessage : Message
    {
        /// <summary>
        /// Creates a new <see cref="ResponseMessage" /> instance.
        /// </summary>
        public ResponseMessage() : this(null, null)
        {

        }

        /// <summary>
        /// Creates a new <see cref="ResponseMessage" /> instance.
        /// </summary>
        public ResponseMessage(object result) : this(result, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ResponseMessage" /> instance.
        /// </summary>
        public ResponseMessage(object result, IResponseError error)
        {
            Version = Constants.JsonRpc.SupportedVersion;
            Result = result;
            Error = error;
        }

        /// <summary>
        /// A unique ID assigned to the request/response session. The request creator is responsible for this value.
        /// </summary>
        [JsonProperty]
        public int Id { get; set; }

        /// <summary>
        /// The error that occurred while processing the request.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IResponseError Error { get; set; }

        /// <summary>
        /// An object representing the result of processing the request.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Result { get; set; }
    }
}
