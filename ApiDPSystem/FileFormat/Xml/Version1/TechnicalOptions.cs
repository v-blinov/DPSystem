using System.Xml.Serialization;

namespace ApiDPSystem.FileFormat.Xml.Version1
{
    [XmlRoot(ElementName = "techincal_options")]
    public class TechnicalOptions
    {
        [XmlElement(ElementName = "engine")]
        public Engine Engine { get; set; }

        [XmlElement(ElementName = "transmission")]
        public string Transmission { get; set; }

        [XmlElement(ElementName = "drive")]
        public string Drive { get; set; }
    }
}