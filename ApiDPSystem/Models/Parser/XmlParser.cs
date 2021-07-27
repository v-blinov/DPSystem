using ApiDPSystem.FileFormat.Xml.Version1;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Xml.Serialization;

namespace ApiDPSystem.Models.Parser
{
    public class XmlParser : Parser
    {
        public override void ProcessFile(IFormFile file)
        {
            string xmlContent;
            var serializer = new XmlSerializer(typeof(Root));

            using (var reader = new StreamReader(file.OpenReadStream()))
                xmlContent = reader.ReadToEnd();

            using (var reader = new StringReader(xmlContent))
            {
                var xmlModel = (Root)serializer.Deserialize(reader);
            }
        }
    }
}
