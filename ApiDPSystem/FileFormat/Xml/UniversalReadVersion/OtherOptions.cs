using System.Collections.Generic;
using System.Xml.Serialization;

namespace ApiDPSystem.FileFormat.Xml.UniversalReadVersion
{
    [XmlRoot(ElementName = "other_options")]
    public record OtherOptions()
    {
        [XmlElement(ElementName = "exterior")]
        public List<string> Exterior { get; set; }

        [XmlElement(ElementName = "interior")]
        public List<string> Interior { get; set; }

        [XmlElement(ElementName = "safety")]
        public List<string> Safety { get; set; }
    }
}