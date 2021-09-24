using System.Xml.Serialization;

namespace ApiDPSystem.FileFormat.Xml.UniversalReadVersion
{
    [XmlRoot(ElementName = "techincal_options")]
    public record TechnicalOptions()
    {
        [XmlElement(ElementName = "engine")]
        public Engine Engine { get; set; }

        [XmlElement(ElementName = "transmission")]
        public string Transmission { get; set; }

        [XmlElement(ElementName = "drive")]
        public string Drive { get; set; }
    }
}