using System.Collections.Generic;
using System.Xml.Serialization;

namespace ApiDPSystem.FileFormat.Xml.Version1
{
    [XmlRoot(ElementName = "cars")]
    public class Car
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "year")]
        public string Year { get; set; }

        [XmlElement(ElementName = "make")]
        public string Make { get; set; }

        [XmlElement(ElementName = "model")]
        public string Model { get; set; }

        [XmlElement(ElementName = "model_trim")]
        public string ModelTrim { get; set; }

        [XmlElement(ElementName = "techincal_options")]
        public TechincalOptions TechincalOptions { get; set; }

        [XmlElement(ElementName = "other_options")]
        public OtherOptions OtherOptions { get; set; }

        [XmlElement(ElementName = "colors")]
        public Colors Colors { get; set; }

        [XmlElement(ElementName = "images")]
        public List<string> Images { get; set; }

        [XmlElement(ElementName = "price")]
        public string Price { get; set; }
    }
}
