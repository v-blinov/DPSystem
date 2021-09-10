using ApiDPSystem.Interfaces;
using System.Collections.Generic;
using System.Linq;
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

        public List<Entities.Car> ConvertToActualDbModel(string dealerName) =>
            Cars.Select(car => car.ConvertToCarActualDbModel(dealerName)).ToList();
    }
}