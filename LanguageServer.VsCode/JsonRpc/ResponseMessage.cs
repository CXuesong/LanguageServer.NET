using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.JsonRpc
{
    /// <summary>
    /// An <see cref="Message" /> implementation representing a JSON-RPC response.
    /// </summary>
    public class ResponseMessage : Message
    {
        /// <summary>
        /// Creates a new <see cref="ResponseMessage" /> instance.
        /// </summary>
        public ResponseMessage()
        {
            Version = Constants.JsonRpc.SupportedVersion;
        }

        /// <summary>
        /// A <see cref="JObject" /> representing an error that occurred while processing the request.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IResponseError Error { get; set; }

        /// <summary>
        /// A unique ID assigned to the request/response session. The request creator is responsible for this value.
        /// </summary>
        [JsonProperty]
        public int Id { get; set; }

        /// <summary>
        /// An object representing the result of processing the request.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Result { get; set; }
    }
}
