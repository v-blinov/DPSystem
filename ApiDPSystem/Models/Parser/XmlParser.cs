using ApiDPSystem.Interfaces;
using System.IO;
using System.Xml.Serialization;

namespace ApiDPSystem.Models.Parser
{
    public class XmlParser<T> : IParser<T>
    {
        public Root<T> DeserializeFile(string fileContent)
        {
            var serializer = new XmlSerializer(typeof(Root<T>));

            using var reader = new StringReader(fileContent);
            return (Root<T>)serializer.Deserialize(reader);
        }

        public void SetDataToDatabase(Root<T> data)
        {
            throw new System.NotImplementedException();
        }
    }
}
