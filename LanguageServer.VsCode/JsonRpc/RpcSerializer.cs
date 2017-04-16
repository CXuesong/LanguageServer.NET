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
            var json = JObject.Parse(content);
            Message message;

            if (json.GetValue("jsonrpc", StringComparison.OrdinalIgnoreCase) == null)
                throw new ArgumentException("Content is not a valid JSON-RPC message.", nameof(content));
            using (var reader = new StringReader(content))
            using (var jreader = new JsonTextReader(reader))
            {
                if (json.GetValue("id", StringComparison.OrdinalIgnoreCase) == null)
                    message = Serializer.Deserialize<NotificationMessage>(jreader);
                else if (json.GetValue("method", StringComparison.OrdinalIgnoreCase) == null)
                    message = Serializer.Deserialize<ResponseMessage>(jreader);
                else
                    message = Serializer.Deserialize<RequestMessage>(jreader);
            }
            return message;
        }

        internal static string SerializeMessage(Message message)
        {
            using (var writer = new StringWriter())
            using (var jwriter = new JsonTextWriter(writer))
            {
                Serializer.Serialize(jwriter, message);
                return writer.ToString();
            }
        }
    }
}
