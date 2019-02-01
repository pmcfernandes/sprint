using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace Sprint.Serializers
{
    public class JsonSerializer : ISerializer
    {
        private static JsonSerializer _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static JsonSerializer Instance
        {
            get
            {
                if (_instance == null) _instance = new JsonSerializer();
                return _instance;
            }
        }

        /// <summary>
        /// Deserializes the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public T Deserialize<T>(string data)
        {
            if (typeof(T) == typeof(ExpandoObject))
            {
                var converter = new ExpandoObjectConverter();
                return JsonConvert.DeserializeObject<T>(data, converter);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
