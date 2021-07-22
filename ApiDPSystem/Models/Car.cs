using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ApiDPSystem.Models
{
    [XmlRoot(ElementName = "cars")]
    public class Car
    {
        [JsonPropertyName("id")]
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }

        [JsonPropertyName("year")]
        [XmlElement(ElementName = "year")]
        public string Year { get; set; }

        [JsonPropertyName("make")]
        [XmlElement(ElementName = "make")]
        public string Make { get; set; }

        [JsonPropertyName("model")]
        [XmlElement(ElementName = "model")]
        public string Model { get; set; }

        [JsonPropertyName("model trim")]
        [XmlElement(ElementName = "model_trim")]
        public string ModelTrim { get; set; }

        [JsonPropertyName("techincal options")]
        [XmlElement(ElementName = "techincal_options")]
        public TechincalOptions TechincalOptions { get; set; }

        [JsonPropertyName("other options")]
        [XmlElement(ElementName = "other_options")]
        public OtherOptions OtherOptions { get; set; }

        [JsonPropertyName("colors")]
        [XmlElement(ElementName = "colors")]
        public Colors Colors { get; set; }

        [JsonPropertyName("images")]
        [XmlElement(ElementName = "images")]
        public List<string> Images { get; set; }

        [JsonPropertyName("price")]
        [XmlElement(ElementName = "price")]
        public string Price { get; set; }
    }
}
