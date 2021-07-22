using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ApiDPSystem.Models
{
    [XmlRoot(ElementName = "root")]
    public class Root
    {
        [JsonPropertyName("version")]
        [XmlElement(ElementName = "version")]
        public int Version { get; set; }

        [JsonPropertyName("cars")]
        [XmlElement(ElementName = "cars")]
        public List<Car> Cars { get; set; }
    }
}
