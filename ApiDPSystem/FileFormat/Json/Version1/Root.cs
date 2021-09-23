using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ApiDPSystem.Interfaces;

namespace ApiDPSystem.FileFormat.Json.Version1
{
    public class Root : IRoot
    {
        public Root()
        {
            Cars = new List<Car>();
        }

        [JsonPropertyName("cars")]
        public List<Car> Cars { get; set; }

        public string FileFormat => ".json";
        public int Version => 1;

        public List<Entities.Car> ConvertToActualDbModel(string dealerName) =>
            Cars.Select(car => car.ConvertCarToDbModel(dealerName)).ToList();
    }
}