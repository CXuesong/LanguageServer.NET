using Newtonsoft.Json.Linq;

namespace VSCode.JsonRpc
{
    /// <summary>
    /// An <see cref="IMessage" /> implementation representing a JSON-RPC notification.
    /// </summary>
    public class NotificationMessage : IMessage
    {
        /// <summary>
        /// Creates a new <see cref="NotificationMessage" /> instance.
        /// </summary>
        public NotificationMessage()
        {
            Version = Constants.JsonRpc.SupportedVersion;
        }

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
