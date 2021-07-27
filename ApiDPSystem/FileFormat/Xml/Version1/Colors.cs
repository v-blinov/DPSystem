using System.Xml.Serialization;

namespace ApiDPSystem.FileFormat.Xml.Version1
{
    [XmlRoot(ElementName = "colors")]
    public class Colors
    {
        [XmlElement(ElementName = "interior")]
        public string Interior { get; set; }

        [XmlElement(ElementName = "exterior")]
        public string Exterior { get; set; }
    }
}
