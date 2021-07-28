using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace ApiDPSystem.Models.Parser
{
    public class Distributer
    {
        public Version JsonGetVersion(string file)
        {
            return JsonSerializer.Deserialize<Version>(file);
        }

        public Version XmlGetVersion(string fileContent)
        {
            var serializer = new XmlSerializer(typeof(Version));

            using var reader = new StringReader(fileContent);
            return (Version)serializer.Deserialize(reader);
        }

        public Version YamlGetVersion(string fileContent)
        {
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                                             .IgnoreUnmatchedProperties()
                                             .Build();
            
            return deserializer.Deserialize<Version>(fileContent);
        }

        public Version CsvGetVersion(string fileName)
        {
            //string xmlContent = ReadFile(file);
            //var serializer = new XmlSerializer(typeof(Version));
            //using (var reader = new StringReader(xmlContent))
            //    return (Version)serializer.Deserialize(reader);
            return new Version() { Value = 1 };
        }
    }
}
