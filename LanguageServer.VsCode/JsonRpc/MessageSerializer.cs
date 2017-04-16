using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace LanguageServer.VsCode.JsonRpc
{
    internal static class MessageSerializer
    {
        private static readonly JsonSerializer serializer = new JsonSerializer
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        internal static Message Deserialize(string content)
        {
            var json = JObject.Parse(content);
            Message message;

            if (json.GetValue("jsonrpc", StringComparison.OrdinalIgnoreCase) == null)
                throw new ArgumentException("Content is not a valid JSON-RPC message.", nameof(content));
            using (var reader = new StringReader(content))
            using (var jreader = new JsonTextReader(reader))
            {
                if (json.GetValue("id", StringComparison.OrdinalIgnoreCase) == null)
                    message = serializer.Deserialize<NotificationMessage>(jreader);
                else if (json.GetValue("method", StringComparison.OrdinalIgnoreCase) == null)
                    message = serializer.Deserialize<ResponseMessage>(jreader);
                else
                    message = serializer.Deserialize<RequestMessage>(jreader);
            }
            return message;
        }

        internal static string Serialize(Message message)
        {
            using (var writer = new StringWriter())
            using (var jwriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jwriter, message);
                return writer.ToString();
            }
        }
    }
}
