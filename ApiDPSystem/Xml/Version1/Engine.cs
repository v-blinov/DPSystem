using System.Xml.Serialization;

namespace ApiDPSystem.Xml.Version1
{
    [XmlRoot(ElementName = "engine")]
    public class Engine
    {
        [XmlElement(ElementName = "fuel")]
        public string Fuel { get; set; }

        [XmlElement(ElementName = "power")]
        public string Power { get; set; }

        [XmlElement(ElementName = "capacity")]
        public string Capacity { get; set; }
    }
}
