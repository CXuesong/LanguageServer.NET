using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using LanguageServer.VsCode.Contracts.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.Contracts
{

    [JsonObject]
    [JsonConverter(typeof(WorkDoneProgressJsonConverter))]
    public abstract class WorkDoneProgress
    {
        internal WorkDoneProgress()
        {
        }

        internal WorkDoneProgress(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Optional, more detailed associated progress message. Contains complementary information to the `title`.
        /// </summary>
        public string Message { get; set; }
    }

    [JsonObject]
    public class WorkDoneProgressBegin : WorkDoneProgress
    {
        internal const string Kind = "begin";

        [JsonConstructor]
        public WorkDoneProgressBegin()
        {
        }

        public WorkDoneProgressBegin(string title)
            : this(title, null, false, null)
        {
        }

        public WorkDoneProgressBegin(string title, bool cancellable, int? percentage)
            : this(title, null, cancellable, percentage)
        {
        }

        public WorkDoneProgressBegin(string title, string message, bool cancellable, int? percentage)
            : base(message)
        {
            Title = title;
            Cancellable = cancellable;
            Percentage = percentage;
        }

        /// <summary>
        /// Mandatory title of the progress operation. Used to briefly inform about
        /// the kind of operation being performed. (Examples: "Indexing" or "Linking dependencies".)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Controls enablement state of a cancel button. This property is only valid if a cancel
        /// button got requested in the `WorkDoneProgressStart` payload.
        /// </summary>
        /// <remarks>Clients that don't support cancellation or don't support control the button's
        /// enablement state are allowed to ignore the setting.</remarks>
        public bool Cancellable { get; set; }

        /// <summary>
        /// Optional progress percentage to display (value 100 is considered 100%).
        /// If not provided infinite progress is assumed and clients are allowed
        /// to ignore the <see cref="WorkDoneProgressReport.Percentage"/> value in subsequent in report notifications.
        /// </summary>
        public int? Percentage { get; set; }
    }

    [JsonObject]
    public class WorkDoneProgressReport : WorkDoneProgress
    {
        internal const string Kind = "report";

        [JsonConstructor]
        public WorkDoneProgressReport()
        {
        }

        public WorkDoneProgressReport(string message)
            : this(message, false, null)
        {
        }

        public WorkDoneProgressReport(string message, int? percentage)
            : this(message, false, percentage)
        {
        }

        public WorkDoneProgressReport(string message, bool cancellable)
            : this(message, cancellable, null)
        {
        }

        public WorkDoneProgressReport(string message, bool cancellable, int? percentage)
            : base(message)
        {
            Cancellable = cancellable;
            Percentage = percentage;
        }

        /// <summary>
        /// Controls enablement state of a cancel button. This property is only valid if a cancel
        /// button got requested in the `WorkDoneProgressStart` payload.
        /// </summary>
        /// <remarks>Clients that don't support cancellation or don't support control the button's
        /// enablement state are allowed to ignore the setting.</remarks>
        public bool Cancellable { get; set; }

        /// <summary>
        /// Optional progress percentage to display (value 100 is considered 100%).
        /// If not provided infinite progress is assumed and clients are allowed
        /// to ignore the <see cref="Percentage"/> value in subsequent in report notifications.
        /// </summary>
        /// <remarks>
        /// he value should be steadily rising. Clients are free to ignore values hat are not following this rule.
        /// </remarks>
        public int? Percentage { get; set; }
    }

    [JsonObject]
    public class WorkDoneProgressEnd : WorkDoneProgress
    {
        internal const string Kind = "end";

        [JsonConstructor]
        public WorkDoneProgressEnd()
        {
        }

        public WorkDoneProgressEnd(string message)
            : base(message)
        {
        }
    }

    public class WorkDoneProgressJsonConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is WorkDoneProgress))
                throw new NotSupportedException();
            var jobj = JObject.FromObject(value, serializer);
            switch (value)
            {
                case WorkDoneProgressBegin _:
                    jobj["kind"] = WorkDoneProgressBegin.Kind;
                    break;
                case WorkDoneProgressReport _:
                    jobj["kind"] = WorkDoneProgressReport.Kind;
                    break;
                case WorkDoneProgressEnd _:
                    jobj["kind"] = WorkDoneProgressEnd.Kind;
                    break;
            }
            jobj.WriteTo(writer);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    reader.Read();
                    return null;
                case JsonToken.StartObject:
                    var jobj = (JObject) JToken.ReadFrom(reader);
                    var kind = (string) jobj["kind"];
                    switch (kind)
                    {
                        case WorkDoneProgressBegin.Kind:
                            return jobj.ToObject<WorkDoneProgressBegin>(serializer);
                        case WorkDoneProgressReport.Kind:
                            return jobj.ToObject<WorkDoneProgressReport>(serializer);
                        case WorkDoneProgressEnd.Kind:
                            return jobj.ToObject<WorkDoneProgressEnd>(serializer);
                        default:
                            throw new NotSupportedException($"Unexpected WorkDoneProgress kind: {kind}.");
                    }
                default:
                    throw new InvalidOperationException($"Unexpected token type. Expect StartObject, actual {reader.TokenType}.");
            }
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return typeof(WorkDoneProgress).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }
    }

    ///// <summary>
    ///// Provides an optional token that a server can use to report work done progress.
    ///// </summary>
    //public interface IWorkDoneProgressParams
    //{
    //    /// <summary>
    //    /// An optional token that a server can use to report work done progress.
    //    /// </summary>
    //    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    //    ProgressToken WorkDoneToken { get; set; }
    //}

    ///// <summary>
    ///// Provides an a parameter literal used to pass a partial result token.
    ///// </summary>
    //public interface IPartialResultParams
    //{
    //    /// <summary>
    //    /// An optional token that a server can use to report partial results (e.g. streaming) to the client.
    //    /// </summary>
    //    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    //    ProgressToken PartialResultToken { get; set; }
    //}

    /// <summary>
    /// Represents a <see cref="string"/>, <see cref="int"/> or <see cref="long"/> value indicating
    /// a token used to report progress.
    /// </summary>
    /// <seealso cref="WorkDoneProgress"/>
    /// <seealso cref="IWindow.CreateWorkDoneProgress"/>
    [JsonConverter(typeof(ProgressTokenJsonConverter))]
    public struct ProgressToken : IEquatable<ProgressToken>
    {

        public static readonly ProgressToken Empty = default;

        public ProgressToken(string value)
        {
            Value = value;
        }

        public ProgressToken(int value)
        {
            Value = value;
        }

        public ProgressToken(long value)
        {
            if (value >= int.MinValue && value <= int.MaxValue)
                Value = (int) value;
            else
                Value = value;
        }

        public ProgressToken(object value)
        {
            if (value is null || value is string || value is int)
                Value = value;
            var longValue = Convert.ToInt64(value);
            if (longValue >= int.MinValue && longValue <= int.MaxValue)
                Value = (int)longValue;
            else
                Value = longValue;
        }

        public object Value { get; }

        public static implicit operator ProgressToken(string rhs)
        {
            return new ProgressToken(rhs);
        }

        public static implicit operator ProgressToken(int rhs)
        {
            return new ProgressToken(rhs);
        }

        public static implicit operator ProgressToken(long rhs)
        {
            return new ProgressToken(rhs);
        }

        public static explicit operator string(ProgressToken rhs)
        {
            return (string) rhs.Value;
        }

        public static explicit operator int(ProgressToken rhs)
        {
            return (int)rhs.Value;
        }

        public static explicit operator long(ProgressToken rhs)
        {
            return (long)rhs.Value;
        }

        /// <inheritdoc />
        public bool Equals(ProgressToken other)
        {
            return Equals(Value, other.Value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is ProgressToken other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
#if BCL_FEATURE_HASHCODE
            return HashCode.Combine(Value);
#else
            return Value != null ? Value.GetHashCode() : 0;
#endif
        }

        public static bool operator ==(ProgressToken left, ProgressToken right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ProgressToken left, ProgressToken right)
        {
            return !left.Equals(right);
        }
    }

    public class ProgressTokenJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ProgressToken);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType != typeof(ProgressToken))
                throw new NotSupportedException();
            return new ProgressToken(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ProgressToken token)
                writer.WriteValue(token.Value);
            else
                throw new NotSupportedException();
        }
    }

}
