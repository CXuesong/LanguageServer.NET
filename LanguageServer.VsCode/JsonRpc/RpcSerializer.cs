using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace LanguageServer.VsCode.JsonRpc
{
    internal static class RpcSerializer
    {
        public static readonly JsonSerializer Serializer = new JsonSerializer
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        internal static Message DeserializeMessage(string content)
        {
            using (var reader = new StringReader(content))
                return DeserializeMessage(reader);
        }

        internal static Message DeserializeMessage(TextReader reader)
        {
            Message message;
            JObject json;
            using (var jreader = new JsonTextReader(reader)) json = JObject.Load(jreader);
            if (json.GetValue("jsonrpc", StringComparison.OrdinalIgnoreCase) == null)
                throw new ArgumentException("Content is not a valid JSON-RPC message.", nameof(reader));
            {
                if (json.GetValue("id", StringComparison.OrdinalIgnoreCase) == null)
                    message = json.ToObject<NotificationMessage>(Serializer);
                else if (json.GetValue("method", StringComparison.OrdinalIgnoreCase) == null)
                    message = json.ToObject<ResponseMessage>(Serializer);
                else
                    message = json.ToObject<RequestMessage>(Serializer);
            }
            return message;
        }

        internal static string SerializeMessage(Message message)
        {
            using (var writer = new StringWriter())
            {
                SerializeMessage(writer, message);
                return writer.ToString();
            }
        }

        internal static void SerializeMessage(TextWriter writer, Message message)
        {
            using (var jwriter = new JsonTextWriter(writer))
            {
                Serializer.Serialize(jwriter, message);
            }
        }
    }
}
