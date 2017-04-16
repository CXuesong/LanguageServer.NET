using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VSCode.JsonRpc
{
    internal static class JsonSerializer
    {
        private static bool _initialized;

        internal static T Deserialize<T>(string json)
        {
            _InitializeConverter();

            return JsonConvert.DeserializeObject<T>(json);
        }

        internal static string Serialize(object value)
        {
            _InitializeConverter();

            return JsonConvert.SerializeObject(value);
        }

        private static void _InitializeConverter()
        {
            if (!_initialized)
            {
                JsonConvert.DefaultSettings = () =>
                {
                    return new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };
                };

                _initialized = true;
            }
        }
    }
}
