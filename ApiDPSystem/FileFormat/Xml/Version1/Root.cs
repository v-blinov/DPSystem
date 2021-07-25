using System.Collections.Generic;
using System.Xml.Serialization;

namespace ApiDPSystem.FileFormat.Xml.Version1
{
    [XmlRoot(ElementName = "root")]
    public class Root
    {
        [XmlElement(ElementName = "version")]
        public int Version { get; set; }

        [XmlElement(ElementName = "cars")]
        public List<Car> Cars { get; set; }
    }
}
