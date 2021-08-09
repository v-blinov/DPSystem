using System.Collections.Generic;
using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;
using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.Version1
{
    public class Root : IRoot
    {
        public Root()
        {
            Cars = new List<Car>();
        }

        [YamlMember(Alias = "cars")]
        public List<Car> Cars { get; set; }

        public string FileFormat => ".yaml";
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