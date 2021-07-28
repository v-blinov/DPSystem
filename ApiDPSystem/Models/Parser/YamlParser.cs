using ApiDPSystem.Interfaces;
using YamlDotNet.Serialization;

namespace ApiDPSystem.Models.Parser
{
    public class YamlParser<T> : IParser<T>
    {
        public Root<T> DeserializeFile(string fileContent)
        {
            var deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
            return deserializer.Deserialize<Root<T>>(fileContent);
        }

        public void SetDataToDatabase(Root<T> data)
        {
            throw new System.NotImplementedException();
        }
    }
}
