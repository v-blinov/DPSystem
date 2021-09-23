using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;

namespace ApiDPSystem.FileFormat.Json.Version1
{
    public class Car : IConvertableToDbCar
    {
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

        public Entities.Car ConvertCarToDbModel(string dealerName)
        {
            var carFeatures = new List<CarFeature>();

            carFeatures.AddRange(IConvertableToDbCar.GetFeaturesCollection(OtherOptions.Exterior, nameof(OtherOptions.Exterior)));
            carFeatures.AddRange(IConvertableToDbCar.GetFeaturesCollection(OtherOptions.Interior, nameof(OtherOptions.Interior)));
            carFeatures.AddRange(IConvertableToDbCar.GetFeaturesCollection(OtherOptions.Safety, nameof(OtherOptions.Safety)));

            var carImages = Images.Select(image => new CarImage { Image = new Image { Url = image } }).ToList();

            var dbConfiguration = new Configuration
            {
                Year = int.Parse(Year),
                Model = Model,
                ModelTrim = ModelTrim,
                Transmission = TechnicalOptions.Transmission,
                Drive = TechnicalOptions.Drive,
                Producer = new Producer { Name = Make },
                Engine = new Entities.Engine
                {
                    Power = int.TryParse(TechnicalOptions.Engine.Power, out var power) ? power : null,
                    Fuel = TechnicalOptions.Engine.Fuel,
                    Capacity = double.TryParse(TechnicalOptions.Engine.Capacity, out var capacity) ? capacity : null
                }
            };

            var dbCar = new Entities.Car
            {
                VinCode = Id,
                Price = int.TryParse(Price, out var price) ? price : null,
                Dealer = new Dealer { Name = dealerName },
                CarImages = carImages,
                InteriorColor = new Color { Name = Colors.Interior },
                ExteriorColor = new Color { Name = Colors.Exterior },
                Configuration = dbConfiguration,
                CarFeatures = carFeatures
            };
            return dbCar;
        }

        public IConvertableToDbCar ConvertFromDbModel(Entities.Car dbModel)
        {
            return new Car()
            {
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
                    Exterior = dbModel.CarFeatures.Select(p => p.Feature).Where(p => p.Type == "Exterior").Select(p => p.Description).ToList(),
                    Interior = dbModel.CarFeatures.Select(p => p.Feature).Where(p => p.Type == "Interior").Select(p => p.Description).ToList(),
                    Safety = dbModel.CarFeatures.Select(p => p.Feature).Where(p => p.Type == "Safety").Select(p => p.Description).ToList(),
                }
            };
        }
    }
}