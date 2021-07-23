using System.Xml.Serialization;

namespace ApiDPSystem.Xml.Version1
{
    [XmlRoot(ElementName = "colors")]
    public class Colors
    {
        [XmlElement(ElementName = "interior")]
        public object Interior { get; set; }

        [XmlElement(ElementName = "exterior")]
        public string Exterior { get; set; }
    }
}
