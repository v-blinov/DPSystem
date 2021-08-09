using ApiDPSystem.Interfaces;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace ApiDPSystem.FileFormat.Csv.Version1
{
    public class Root : ICsvRoot
    {
        public Root()
        {
            Cars = new List<Car>();
        }

        public string FileFormat => ".csv";
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

        public IRoot Deserialize(string fileContent)
        {
            var root = new Root();

            using var reader = new StringReader(fileContent);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            while (csv.Read())
            {
                var car = csv.GetRecord<Car>();

                var headers = csv.HeaderRecord.ToList();

                var images_FieldNames = headers.Where(p => p.StartsWith("images/")).ToList();
                car.Images = images_FieldNames.Select(name => csv.GetField(name)).ToList();

                var otherOptions_Multimedia_FieldNames = headers.Where(p => p.StartsWith("other options/multimedia/")).ToList();
                car.OtherOptions.Multimedia = otherOptions_Multimedia_FieldNames.Select(name => csv.GetField(name)).ToList();

                var otherOptions_Interior_FieldNames = headers.Where(p => p.StartsWith("other options/interior/")).ToList();
                car.OtherOptions.Interior = otherOptions_Interior_FieldNames.Select(name => csv.GetField(name)).ToList();

                var otherOptions_Exterior_FieldNames = headers.Where(p => p.StartsWith("other options/exterior/")).ToList();
                car.OtherOptions.Exterior = otherOptions_Exterior_FieldNames.Select(name => csv.GetField(name)).ToList();

                root.Cars.Add(car);
            }
            return root;
        }
    }
}
