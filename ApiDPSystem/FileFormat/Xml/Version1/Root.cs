using System.Collections.Generic;
using System.Xml.Serialization;
using ApiDPSystem.Entities;
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

        public List<CarActual> ConvertToActualDbModel(string dealerName)
        {
            var dbModels = new List<CarActual>();

            foreach (var car in Cars)
                dbModels.Add(car.ConvertToCarActualDbModel(dealerName));

            return dbModels;
        }
    }
}