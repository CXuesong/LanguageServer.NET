using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    ///  A `MarkupContent` literal represents a string value which content is interpreted base on its
    ///  kind flag. Currently the protocol supports <c>plaintext</c> and <c>markdown</c> as markup kinds.
    ///  If the kind is `markdown` then the value can contain fenced code blocks like in GitHub issues.
    ///  See <a href="https://help.github.com/articles/creating-and-highlighting-code-blocks/#syntax-highlighting">https://help.github.com/articles/creating-and-highlighting-code-blocks/#syntax-highlighting</a>.
    /// </summary>
    [JsonObject]
    public class MarkupContent
    {

        /// <summary>
        /// Creates a <see cref="MarkupContent"/> with plaintext content.
        /// </summary>
        public static MarkupContent PlainText(string value) => new MarkupContent(MarkupKind.PlainText, value);

        /// <summary>
        /// Creates a <see cref="MarkupContent"/> with markdown content.
        /// </summary>
        public static MarkupContent Markdown(string value) => new MarkupContent(MarkupKind.Markdown, value);

        /// <summary>
        /// Creates a <see cref="MarkupContent"/> with a code snippet in the given language.
        /// This is equivalent to the deprecated <c>MarkedString</c>.
        /// </summary>
        /// <param name="language">the code language.</param>
        /// <param name="value">the code.</param>
        /// <remarks>
        /// <para><c>MarkedString</c> can be used to render human readable text. It is either a markdown string
        /// or a code-block that provides a language and a code snippet. The language identifier
        /// is semantically equal to the optional language identifier in fenced code blocks in GitHub
        /// issues. See https://help.github.com/articles/creating-and-highlighting-code-blocks/#syntax-highlighting</para>
        /// <para>We still keep this overload in LanguageServer.NET in order to facilitate the code snippet construction.</para>
        /// </remarks>
#if BCL_FEATURE_SPAN
        public static MarkupContent MarkedString(string language, string value) => MarkedString(language, value.AsSpan());

        /// <inheritdoc cref="MarkedString(string,string)"/>
        public static MarkupContent MarkedString(string language, ReadOnlySpan<char> value)
#else
        public static MarkupContent MarkedString(string language, string value)
#endif
        {
            var sb = new StringBuilder(language.Length + value.Length + 10);
            sb.Append('`', 4);
            sb.Append(language);
            sb.Append('\n');
            sb.Append(value);
            sb.Append('\n');
            sb.Append('`', 4);
            return new MarkupContent(MarkupKind.Markdown, sb.ToString());
        }

        /// <exception cref="ArgumentNullException"><paramref name="kind"/> is <c>null</c>.</exception>
        public MarkupContent(MarkupKind kind, string value)
        {
            Kind = kind ?? throw new ArgumentNullException(nameof(kind));
            Value = value;
        }

        /// <summary>The type of the Markup.</summary>
        [JsonProperty]
        public MarkupKind Kind { get; }

        /// <summary>The content itself.</summary>
        [JsonProperty]
        public string Value { get; }

        /// <summary>
        /// Implicitly converts string into <see cref="MarkupContent"/> with <see cref="PlainText"/>.
        /// </summary>
        public static implicit operator MarkupContent(string value) => PlainText(value);

    }

    /// <summary>
    /// Describes the content type that a client supports in various
    /// result literals like <see cref="Hover"/>, <see cref="ParameterInformation"/> or <see cref="CompletionItem"/>.
    /// </summary>
    /// <remarks>
    /// Please note that <see cref="MarkupKind"/> must not start with a <c>$</c>. This kinds
    /// are reserved for internal usage.
    /// </remarks>
    [JsonConverter(typeof(MarkupKindJsonConverter))]
    public sealed class MarkupKind : IEquatable<MarkupKind>
    {

        /// <summary>
        /// Plain text is supported as a content format.
        /// </summary>
        public static MarkupKind PlainText { get; } = new MarkupKind("plaintext");

        /// <summary>
        /// Markdown is supported as a content format.
        /// </summary>
        public static MarkupKind Markdown { get; } = new MarkupKind("markdown");

        public MarkupKind(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the string representation of the markup kind.
        /// </summary>
        public string Value { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value;
        }

        /// <inheritdoc />
        public bool Equals(MarkupKind other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Value, other.Value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is MarkupKind && Equals((MarkupKind)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(MarkupKind left, MarkupKind right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MarkupKind left, MarkupKind right)
        {
            return !Equals(left, right);
        }
    }

    /// <summary>
    /// Used to convert <see cref="MarkupKind"/> into JSON.
    /// </summary>
    public class MarkupKindJsonConverter : JsonConverter
    {

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var v = (MarkupKind)value;
            writer.WriteValue(v.Value);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                throw new InvalidOperationException($"Cannot parse MarkupKind: Expect string token. Get {reader.TokenType}.");
            var v = (string) reader.Value;
            if (v == null) return null;
            if (v == "plaintext") return MarkupKind.PlainText;
            if (v == "markdown") return MarkupKind.Markdown;
            return new MarkupKind(v);
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MarkupKind);
        }
    }

}
