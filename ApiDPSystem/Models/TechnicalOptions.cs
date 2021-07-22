using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ApiDPSystem.Models
{
    [XmlRoot(ElementName = "techincal_options")]
    public class TechincalOptions
    {
        [JsonPropertyName("engine")]
        [XmlElement(ElementName = "engine")]
        public Engine Engine { get; set; }

        [JsonPropertyName("transmission")]
        [XmlElement(ElementName = "transmission")]
        public string Transmission { get; set; }

        [JsonPropertyName("drive")]
        [XmlElement(ElementName = "drive")]
        public string Drive { get; set; }
    }
}
