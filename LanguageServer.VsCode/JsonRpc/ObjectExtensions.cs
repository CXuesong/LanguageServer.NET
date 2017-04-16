using Newtonsoft.Json.Linq;

namespace VSCode.JsonRpc
{
    /// <summary>
    /// Provides object extensions for converting .NET objects to JSON objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts this instance to a <see cref="JObject" />.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static JObject ToJObject(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            string json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<JObject>(json);
        }
    }
}
