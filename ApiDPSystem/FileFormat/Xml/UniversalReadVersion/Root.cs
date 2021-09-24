using System.Collections.Generic;
using System.Xml.Serialization;

namespace ApiDPSystem.FileFormat.Xml.UniversalReadVersion
{
    [XmlRoot(ElementName = "root")]
    public record Root()
    {
        [XmlElement(ElementName = "cars")]
        public List<Car> Cars { get; set; }
    }
}