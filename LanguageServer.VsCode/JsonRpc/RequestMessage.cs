using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.JsonRpc
{
    /// <summary>
    /// An <see cref="Message" /> implementation representing a JSON-RPC request.
    /// </summary>
    public class RequestMessage : Message
    {
        /// <summary>
        /// Creates a new <see cref="RequestMessage" /> instance.
        /// </summary>
        public RequestMessage()
        {
            Version = Constants.JsonRpc.SupportedVersion;
        }

        /// <summary>
        /// A unique ID given to the request/response session. The request creator is responsible for assigning this value.
        /// </summary>
        [JsonProperty]
        public int Id { get; set; }

        /// <summary>
        /// The method to invoke on the receiver.
        /// </summary>
        [JsonProperty]
        public string Method { get; set; }

        /// <summary>
        /// A <see cref="JObject" /> representing parameters for the method.
        /// </summary>
        [JsonProperty]
        public JObject Params { get; set; }
    }
}
