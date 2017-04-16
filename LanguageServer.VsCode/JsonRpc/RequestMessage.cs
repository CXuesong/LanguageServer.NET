using Newtonsoft.Json.Linq;

namespace VSCode.JsonRpc
{
    /// <summary>
    /// An <see cref="IMessage" /> implementation representing a JSON-RPC request.
    /// </summary>
    public class RequestMessage : IMessage
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
        public int Id { get; set; }

        /// <summary>
        /// See <see cref="IMessage.Version" />.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The method to invoke on the receiver.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// A <see cref="JObject" /> representing parameters for the method.
        /// </summary>
        public JObject Params { get; set; }
    }
}
