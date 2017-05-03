using System;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    public enum MessageType
    {
        /// <summary>
        /// An error message.
        /// </summary>
        Error = 1,

        /// <summary>
        /// A warning message.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// An information message.
        /// </summary>
        Info = 3,

        /// <summary>
        /// A log message.
        /// </summary>
        Log = 4,
    }

    /// <summary>
    /// Represents an action button shown in the message box (or popup bar).
    /// </summary>
    public interface IMessageActionItem
    {
        /// <summary>
        /// A short title like 'Retry', 'Open Log' etc.
        /// </summary>
        string Title { get; }
    }

    /// <summary>
    /// Provides a handy structure for covnersion between string and <see cref="IMessageActionItem"/>.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
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

        /// <inheritdoc />
        [JsonProperty]
        public string Title { get; }

        /// <inheritdoc />
        public bool Equals(MessageActionItem other)
        {
            return string.Equals((string) Title, (string) other.Title);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(null, obj)) return false;
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
}