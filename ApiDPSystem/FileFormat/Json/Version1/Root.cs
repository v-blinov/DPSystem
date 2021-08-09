using ApiDPSystem.Interfaces;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiDPSystem.FileFormat.Json.Version1
{
    public class Root : IRoot
    {
        public Root()
        {
            Cars = new List<Car>();
        }

        public string FileFormat => ".json";
        public int Version => 1;

        [JsonPropertyName("cars")]
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
