using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration.Attributes;

namespace ApiDPSystem.FileFormat.Csv.UniversalReadVersion
{
    public record Car()
    {
        [Name("is actual")]
        public bool IsActual { get; set; }

        [Name("is sold")]
        public bool IsSold { get; set; }
        
        [Name("id")]
        public string Id { get; set; }

        [Name("year")]
        public string Year { get; set; }

        [Name("make")]
        public string Make { get; set; }

        [Name("model")]
        public string Model { get; set; }

        [Name("model trim")]
        public string ModelTrim { get; set; }

        public TechnicalOptions TechnicalOptions { get; set; }

        public OtherOptions OtherOptions { get; set; }

        public Colors Colors { get; set; }

        public List<string> Images { get; set; }

        [Name("price")]
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
                    Multimedia = GetFeatureListOrNull(dbModel, nameof(Car.OtherOptions.Multimedia))
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
    }
}