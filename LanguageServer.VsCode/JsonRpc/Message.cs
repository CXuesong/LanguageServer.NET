using System;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.JsonRpc
{
    /// <summary>
    /// Represents the base abstract JSON-RPC message.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Message
    {
        /// <summary>
        /// The version of the JSON-RPC specification in use.
        /// </summary>
        /// <remarks>
        /// <note type="note">This property is not used in version 1.0 of the JSON-RPC specification. As of version 2.0, the value should always be "2.0".</note>
        /// </remarks>
        [JsonProperty("jsonrpc")]
        public string Version { get; set; }
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            Message = message;
        }

        public Message Message { get; }

        public ResponseMessage Response { get; set; }
    }

    public interface IMessageSource
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }
}
