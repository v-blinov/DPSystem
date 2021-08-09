using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiDPSystem.Entities;
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

        public List<CarActual> ConvertToActualDbModel(string dealerName)
        {
            var dbModels = new List<CarActual>();

            foreach (var car in Cars)
                dbModels.Add(car.ConvertToCarActualDbModel(dealerName));

            return dbModels;
        }
    }
}