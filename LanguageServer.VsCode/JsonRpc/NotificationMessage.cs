using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.JsonRpc
{
    /// <summary>
    /// An <see cref="Message" /> implementation representing a JSON-RPC notification.
    /// </summary>
    public class NotificationMessage : Message
    {
        /// <summary>
        /// Creates a new <see cref="NotificationMessage" /> instance.
        /// </summary>
        public NotificationMessage()
        {
            Version = Constants.JsonRpc.SupportedVersion;
        }

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

        public object GetParams(Type paramsType)
        {
            return Params?.ToObject(paramsType, RpcSerializer.Serializer);
        }

        public T GetParams<T>()
        {
            return Params == null ? default(T) : Params.ToObject<T>(RpcSerializer.Serializer);
        }
    }
}
