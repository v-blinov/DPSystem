using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ApiDPSystem.Models
{
    [XmlRoot(ElementName = "other_options")]
    public class OtherOptions
    {
        [JsonPropertyName("exterior")]
        [XmlElement(ElementName = "exterior")]
        public object Exterior { get; set; }

        [JsonPropertyName("interior")]
        [XmlElement(ElementName = "interior")]
        public List<string> Interior { get; set; }

        [JsonPropertyName("safety")]
        [XmlElement(ElementName = "safety")]
        public List<string> Safety { get; set; }
    }
}
