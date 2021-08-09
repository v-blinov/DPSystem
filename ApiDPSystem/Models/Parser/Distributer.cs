using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using YamlDotNet.Serialization;

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
            return (Version) serializer.Deserialize(reader);
        }

        public Version YamlGetVersion(string fileContent)
        {
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();

            return deserializer.Deserialize<Version>(fileContent);
        }

        public Version CsvGetVersion(string fileName)
        {
            var regex = new Regex(@"_v(\d)+\.");
            var matches = regex.Matches(fileName);

            if (matches.Count > 0)
            {
                var versionString = matches.Last().Value.Replace("_v", "").Replace(".", "");
                return new Version {Value = Convert.ToInt32(versionString)};
            }

            return new Version();
        }
    }
}