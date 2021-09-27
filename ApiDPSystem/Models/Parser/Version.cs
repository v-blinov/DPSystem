using System.Text.Json.Serialization;
using System.Xml.Serialization;
using CsvHelper.Configuration.Attributes;
using YamlDotNet.Serialization;

namespace ApiDPSystem.Models.Parser
{
    [XmlRoot(ElementName = "root")]
    public class Version
    {
        [JsonPropertyName("version")]
        [XmlElement(ElementName = "version")]
        [YamlMember(Alias = "version")]
        [Name("version")]
        public int Value { get; set; }
    }
}