using System.Xml.Serialization;

namespace ApiDPSystem.FileFormat.Xml.UniversalReadVersion
{
    [XmlRoot(ElementName = "colors")]
    public record Colors()
    {
        [XmlElement(ElementName = "interior")]
        public string Interior { get; set; }

        [XmlElement(ElementName = "exterior")]
        public string Exterior { get; set; }
    }
}