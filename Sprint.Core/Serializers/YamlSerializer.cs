using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Sprint.Serializers
{
    public class YamlSerializer : ISerializer
    {
        private static YamlSerializer _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static YamlSerializer Instance
        {
            get
            {
                if (_instance == null) _instance = new YamlSerializer();
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
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            using (var reader = new StringReader(data))
            {
                return deserializer.Deserialize<T>(reader);
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
            var serializer = new SerializerBuilder()
                .Build();

            return serializer.Serialize(obj);
        }

        /// <summary>
        /// Serializes to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public string SerializeToJson<T>(T obj)
        {
            var serializer = new SerializerBuilder()
                .JsonCompatible()
                .Build();

            return serializer.Serialize(obj);
        }
    }
}
