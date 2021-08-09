using ApiDPSystem.Interfaces;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.Version1
{
    public class Root : IRoot
    {
        public Root()
        {
            Cars = new List<Car>();
        }

        public string FileFormat => ".yaml";
        public int Version => 1;

        [YamlMember(Alias = "cars")]
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
