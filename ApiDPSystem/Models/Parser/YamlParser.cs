using ApiDPSystem.FileFormat.Yaml.Version1;
using Microsoft.AspNetCore.Http;
using System.IO;
using YamlDotNet.Serialization;

namespace ApiDPSystem.Models.Parser
{
    public class YamlParser : Parser
    {
        public override void ProcessFile(IFormFile file)
        {
            string yamlContent;
            var deserializer = new DeserializerBuilder().Build();

            using (var reader = new StreamReader(file.OpenReadStream()))
                yamlContent = reader.ReadToEnd();

            var yamlModel = deserializer.Deserialize<Root>(yamlContent);
        }
    }
}
