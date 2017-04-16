using Newtonsoft.Json;

namespace VSCode.JsonRpc
{
    /// <summary>
    /// Represents the base abstract JSON-RPC message.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// The version of the JSON-RPC specification in use.
        /// </summary>
        /// <remarks>
        /// <note type="note">This property is not used in version 1.0 of the JSON-RPC specification. As of version 2.0, the value should always be "2.0".</note>
        /// </remarks>
        [JsonProperty("jsonrpc")]
        string Version { get; set; }
    }
}
