using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ApiDPSystem.Models
{
    [XmlRoot(ElementName = "engine")]
    public class Engine
    {
        [JsonPropertyName("fuel")]
        [XmlElement(ElementName = "fuel")]
        public string Fuel { get; set; }

        [JsonPropertyName("power")]
        [XmlElement(ElementName = "power")]
        public string Power { get; set; }

        [JsonPropertyName("capacity")]
        [XmlElement(ElementName = "capacity")]
        public string Capacity { get; set; }
    }
}
