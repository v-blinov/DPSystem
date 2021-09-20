using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using ApiDPSystem.Interfaces;
using CsvHelper;

namespace ApiDPSystem.FileFormat.Csv.Version2
{
    public class Root : ICsvRoot
    {
        public Root()
        {
            Cars = new List<Car>();
        }

        [JsonPropertyName("cars")]
        public List<Car> Cars { get; set; }

        public string FileFormat => ".csv";
        public int Version => 2;

        public List<Entities.Car> ConvertToActualDbModel(string dealerName) =>
            Cars.Select(car => car.ConvertToCarActualDbModel(dealerName)).ToList();

        public IRoot Deserialize(string fileContent)
        {
            var root = new Root();

            using var reader = new StringReader(fileContent);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            while (csv.Read())
            {
                var car = csv.GetRecord<Car>();

                var headers = csv.HeaderRecord.ToList();

                var imagesFieldNames = headers.Where(p => p.StartsWith("images/")).ToList();
                car.Images = imagesFieldNames.Select(name => csv.GetField(name)).ToList();

                var safetyFieldNames = headers.Where(p => p.StartsWith("other options/safety/")).ToList();
                car.OtherOptions.Safety = safetyFieldNames.Select(name => csv.GetField(name)).ToList();

                var exteriorFieldNames = headers.Where(p => p.StartsWith("other options/exterior/")).ToList();
                car.OtherOptions.Exterior = exteriorFieldNames.Select(name => csv.GetField(name)).ToList();

                root.Cars.Add(car);
            }

            return root;
        }
    }
}