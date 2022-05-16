using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.UniversalReadVersion
{
    public record Car
    {
        [YamlMember(Alias = "is_actual")]
        public bool IsActual { get; set; }

        [YamlMember(Alias = "is_sold")]
        public bool IsSold { get; set; }

        [YamlMember(Alias = "id")]
        public string Id { get; set; }

        [YamlMember(Alias = "year")]
        public string Year { get; set; }

        [YamlMember(Alias = "make")]
        public string Make { get; set; }

        [YamlMember(Alias = "model")]
        public string Model { get; set; }

        [YamlMember(Alias = "model trim")]
        public string ModelTrim { get; set; }

        [YamlMember(Alias = "techincal options")]
        public TechnicalOptions TechnicalOptions { get; set; }

        [YamlMember(Alias = "other options")]
        public OtherOptions OtherOptions { get; set; }

        [YamlMember(Alias = "colors")]
        public Colors Colors { get; set; }

        [YamlMember(Alias = "images")]
        public List<string> Images { get; set; }

        [YamlMember(Alias = "price")]
        public string Price { get; set; }


        public static Car ConvertFromDbModel(Entities.Car dbModel) =>
            new Car
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

                Colors = new Colors
                {
                    Exterior = dbModel.ExteriorColor.Name,
                    Interior = dbModel.InteriorColor.Name
                },
                TechnicalOptions = new TechnicalOptions
                {
                    Drive = dbModel.Configuration.Drive,
                    Engine = new Engine
                    {
                        Fuel = dbModel.Configuration.Engine.Fuel,
                        Power = dbModel.Configuration.Engine.Power.ToString(),
                        Capacity = dbModel.Configuration.Engine.Capacity.ToString()
                    },
                    Transmission = dbModel.Configuration.Transmission
                },
                OtherOptions = new OtherOptions
                {
                    Exterior = GetFeatureListOrNull(dbModel, nameof(Car.OtherOptions.Exterior)),
                    Interior = GetFeatureListOrNull(dbModel, nameof(Car.OtherOptions.Interior)),
                    Safety = GetFeatureListOrNull(dbModel, nameof(Car.OtherOptions.Safety))
                }
            };

        private static List<string> GetFeatureListOrNull(Entities.Car model, string optionsTypeName)
        {
            var features = model.CarFeatures
                                .Select(p => p.Feature)
                                .Where(p => p.Type == optionsTypeName)
                                .Select(p => p.Description)
                                .ToList();

            return features.Count > 0 ? features : null;
        }
    }
}