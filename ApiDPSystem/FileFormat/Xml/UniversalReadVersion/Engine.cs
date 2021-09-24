using System.Xml.Serialization;

namespace ApiDPSystem.FileFormat.Xml.UniversalReadVersion
{
    [XmlRoot(ElementName = "engine")]
    public record Engine()
    {
        [XmlElement(ElementName = "fuel")]
        public string Fuel { get; set; }

        [XmlElement(ElementName = "power")]
        public string Power { get; set; }

        [XmlElement(ElementName = "capacity")]
        public string Capacity { get; set; }
    }
}