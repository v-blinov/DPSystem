using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ApiDPSystem.Models
{
    [XmlRoot(ElementName = "colors")]
    public class Colors
    {
        [JsonPropertyName("interior")]
        [XmlElement(ElementName = "interior")]
        public object Interior { get; set; }

        [JsonPropertyName("exterior")]
        [XmlElement(ElementName = "exterior")]
        public string Exterior { get; set; }
    }
}
