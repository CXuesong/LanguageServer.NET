using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonRpc.Standard.Contracts;

namespace LanguageServer.VsCode.Contracts.Client
{
    public enum MessageType
    {
        /**
 * An error message.
 */
        Error = 1,

        /**
         * A warning message.
         */
        Warning = 2,

        /**
         * An information message.
         */
        Info = 3,

        /**
         * A log message.
         */
        Log = 4,
    }

    public interface IMessageActionItem
    {
        /// <summary>
        /// A short title like 'Retry', 'Open Log' etc.
        /// </summary>
        string Title { get; }
    }

    public struct MessageActionItem : IMessageActionItem, IEquatable<MessageActionItem>
    {
        public MessageActionItem(string title)
        {
            Title = title;
        }

        public static implicit operator string(MessageActionItem x)
        {
            return x.Title;
        }

        public static implicit operator MessageActionItem(string x)
        {
            return new MessageActionItem(x);
        }

        public static bool operator ==(MessageActionItem x, MessageActionItem y)
        {
            return x.Title == y.Title;
        }

        public static bool operator !=(MessageActionItem x, MessageActionItem y)
        {
            return x.Title != y.Title;
        }

        public static bool operator ==(string x, MessageActionItem y)
        {
            return y.Title == x;
        }

        public static bool operator !=(string x, MessageActionItem y)
        {
            return y.Title != x;
        }

        public static bool operator ==(MessageActionItem x, string y)
        {
            return x.Title == y;
        }

        public static bool operator !=(MessageActionItem x, string y)
        {
            return x.Title != y;
        }

        /// <summary>
        /// A short title like 'Retry', 'Open Log' etc.
        /// </summary>
        public string Title { get; }

        /// <inheritdoc />
        public bool Equals(MessageActionItem other)
        {
            return string.Equals(Title, other.Title);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MessageActionItem && Equals((MessageActionItem) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Title != null ? Title.GetHashCode() : 0;
        }

        /// <inheritdoc />
        public override string ToString() => Title;
    }

    [JsonRpcScope(MethodPrefix = "window/")]
    public interface IWindow
    {
        /// <summary>
        /// The show message notification is sent from a server to a client to ask the client to display a particular message in the user interface.
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
        Task<MessageActionItem> ShowMessage(MessageType type, string message, params MessageActionItem[] actions);

        /// <summary>
        /// The log message notification is sent from the server to the client to ask the client to log a particular message.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="message">The actual message.</param>
        [JsonRpcMethod(IsNotification = true)]
        Task LogMessage(MessageType type, string message);
    }
}
