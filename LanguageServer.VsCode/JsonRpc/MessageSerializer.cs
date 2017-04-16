using System;

using Newtonsoft.Json.Linq;

namespace VSCode.JsonRpc
{
    internal static class MessageSerializer
    {
        private static bool _initialized;

        internal static IMessage Deserialize(string content)
        {
            JObject json = JObject.Parse(content);
            IMessage message = null;

            if (json.GetValue("jsonrpc", StringComparison.CurrentCultureIgnoreCase) == null)
            {
                throw new ArgumentException("Content is not a valid JSON-RPC message.", "content");
            }

            if (json.GetValue("id", StringComparison.CurrentCultureIgnoreCase) == null)
            {
                message = JsonSerializer.Deserialize<NotificationMessage>(content);
            }

            else if (json.GetValue("method", StringComparison.CurrentCultureIgnoreCase) == null)
            {
                message = JsonSerializer.Deserialize<ResponseMessage>(content);
            }

            else
            {
                message = JsonSerializer.Deserialize<RequestMessage>(content);
            }

            return message;
        }

        internal static string Serialize(IMessage message)
        {
            return JsonSerializer.Serialize(message);
        }
    }
}
