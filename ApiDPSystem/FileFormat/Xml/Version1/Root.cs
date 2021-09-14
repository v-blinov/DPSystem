using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ApiDPSystem.Interfaces;

namespace ApiDPSystem.FileFormat.Xml.Version1
{
    [XmlRoot(ElementName = "root")]
    public class Root : IRoot
    {
        public Root()
        {
            Cars = new List<Car>();
        }

        [XmlElement(ElementName = "cars")]
        public List<Car> Cars { get; set; }

        public string FileFormat => ".xml";
        public int Version => 1;

        public List<Entities.Car> ConvertToActualDbModel(string dealerName) =>
            Cars.Select(car => car.ConvertToCarActualDbModel(dealerName)).ToList();
    }
}