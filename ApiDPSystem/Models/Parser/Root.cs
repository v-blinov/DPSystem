using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using YamlDotNet.Serialization;

namespace ApiDPSystem.Models.Parser
{
    [XmlRoot(ElementName = "root")]
    public class Root<T>
    {
        public Root()
        {
            Cars = new List<T>();
        }


        [JsonPropertyName("cars")]
        [XmlElement(ElementName = "cars")]
        [YamlMember(Alias = "cars")]
        public List<T> Cars { get; set; }
    }
}
