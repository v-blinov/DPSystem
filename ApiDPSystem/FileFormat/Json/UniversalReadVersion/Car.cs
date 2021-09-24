using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ApiDPSystem.FileFormat.Json.UniversalReadVersion
{
    public record Car()
    {
        [JsonPropertyName("is actual")]
        public bool IsActual { get; set; }

        [JsonPropertyName("is sold")]
        public bool IsSold { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("year")]
        public string Year { get; set; }

        [JsonPropertyName("make")]
        public string Make { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("model trim")]
        public string ModelTrim { get; set; }

        [JsonPropertyName("techincal options")]
        public TechnicalOptions TechnicalOptions { get; set; }

        [JsonPropertyName("other options")]
        public OtherOptions OtherOptions { get; set; }

        [JsonPropertyName("colors")]
        public Colors Colors { get; set; }

        [JsonPropertyName("images")]
        public List<string> Images { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        public static Car ConvertFromDbModel(Entities.Car dbModel)
        {
            return new Car()
            {
                IsActual = dbModel.IsActual,
                IsSold = dbModel.IsSold,
                Id = dbModel.VinCode,
                Price = dbModel.Price.ToString(),
                Year = dbModel.Configuration.Year.ToString(),
                Model = dbModel.Configuration.Model,
                ModelTrim = dbModel.Configuration.ModelTrim,
                Make = dbModel.Configuration.Producer.Name,
                Images = dbModel.CarImages.Select(p => p.Image.Url).ToList(),

                Colors = new Colors()
                {
                    Exterior = dbModel.ExteriorColor.Name,
                    Interior = dbModel.InteriorColor.Name,
                },
                TechnicalOptions = new TechnicalOptions()
                {
                    Drive = dbModel.Configuration.Drive,
                    Engine = new Engine()
                    {
                        Fuel = dbModel.Configuration.Engine.Fuel,
                        Power = dbModel.Configuration.Engine.Power.ToString(),
                        Capacity = dbModel.Configuration.Engine.Capacity.ToString(),
                    },
                    Transmission = dbModel.Configuration.Transmission,
                },
                OtherOptions = new OtherOptions()
                {
                    Exterior = GetFeatureListOrNull(dbModel, nameof(Car.OtherOptions.Exterior)),
                    Interior = GetFeatureListOrNull(dbModel, nameof(Car.OtherOptions.Interior)),
                    Safety = GetFeatureListOrNull(dbModel, nameof(Car.OtherOptions.Safety)),
                    Multimedia = GetFeatureListOrNull(dbModel, nameof(Car.OtherOptions.Multimedia)),
                    Comfort = GetFeatureListOrNull(dbModel, nameof(Car.OtherOptions.Comfort)),
                }
            };
        }

        private static List<string> GetFeatureListOrNull(Entities.Car model, string optionsTypeName)
        {
            var features = model.CarFeatures
                                .Select(p => p.Feature)
                                .Where(p => p.Type == optionsTypeName)
                                .Select(p => p.Description)
                                .ToList();

            return features.Count > 0 ? features : null;
        }
    };
}