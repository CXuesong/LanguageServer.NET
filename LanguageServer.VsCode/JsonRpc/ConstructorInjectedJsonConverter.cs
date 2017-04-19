using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.JsonRpc
{
    internal class ConstructorInjectedJsonConverter<T> : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException($"Serialization of {value.GetType()} to JSON is not supported.");
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var token = JToken.ReadFrom(reader);
            ConstructorInfo ctor;
            try
            {
                ctor = objectType.GetTypeInfo()
                    .DeclaredConstructors.First(c =>
                    {
                        var pa = c.GetParameters();
                        return pa.Length == 1 && pa[0].ParameterType == typeof(JToken);
                    });
            }
            catch (InvalidOperationException)
            {
                throw new MissingMethodException($"Cannot find JToken injectable constructor on \"{objectType}\".");
            }
            return (T) ctor.Invoke(new object[] {token});
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }
    }
}
