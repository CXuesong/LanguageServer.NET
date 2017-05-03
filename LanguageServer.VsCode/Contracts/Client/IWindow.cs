using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonRpc.Standard.Contracts;

namespace LanguageServer.VsCode.Contracts.Client
{

    [JsonRpcScope(MethodPrefix = "window/")]
    public interface IWindow
    {
        /// <summary>
        /// The show message notification is sent from a server to a client to ask the client to display
        /// a particular message in the user interface.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="message">The actual message.</param>
        [JsonRpcMethod(IsNotification = true)]
        Task ShowMessage(MessageType type, string message);

        /// <summary>
        /// The show message request is sent from a server to a client to ask the client to
        /// display a particular message in the user interface. In addition to the show
        /// message notification the request allows to pass actions and to wait for an answer from the client.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="message">The actual message.</param>
        /// <param name="actions">The message action items to present.</param>
        [JsonRpcMethod("showMessageRequest")]
        Task<IMessageActionItem> ShowMessage(MessageType type, string message, IEnumerable<IMessageActionItem> actions);

        /// <summary>
        /// The show message request is sent from a server to a client to ask the client to
        /// display a particular message in the user interface. In addition to the show
        /// message notification the request allows to pass actions and to wait for an answer from the client.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="message">The actual message.</param>
        /// <param name="actions">The message action items to present.</param>
        [JsonRpcMethod("showMessageRequest")]
        Task<MessageActionItem?> ShowMessage(MessageType type, string message, params MessageActionItem[] actions);

        /// <summary>
        /// The log message notification is sent from the server to the client to ask the client to log a particular message.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="message">The actual message.</param>
        [JsonRpcMethod(IsNotification = true)]
        Task LogMessage(MessageType type, string message);
    }
}
