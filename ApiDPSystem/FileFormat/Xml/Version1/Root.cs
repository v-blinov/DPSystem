﻿using ApiDPSystem.Interfaces;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ApiDPSystem.FileFormat.Xml.Version1
{
    [XmlRoot(ElementName = "root")]
    public class Root : IRoot
    {
        public Root()
        {
            Cars = new List<Car>();
        }

        public string FileFormat => ".xml";
        public int Version => 1;

        [XmlElement(ElementName = "cars")]
        public List<Car> Cars { get; set; }

        public List<Entities.CarActual> ConvertToActualDbModel(string dealerName)
        {
            var dbModels = new List<Entities.CarActual>();

            foreach (var car in Cars)
                dbModels.Add(car.ConvertToCarActualDbModel(dealerName));

            return dbModels;
        }
    }
}
