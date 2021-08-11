using System.Collections.Generic;
using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;
using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.Version1
{
    public class Car : IConvertableToDbCar
    {
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

        public CarActual ConvertToCarActualDbModel(string dealerName)
        {
            var configurationFeatures = new List<ConfigurationFeature>();

            configurationFeatures.AddRange(IConvertableToDbCar.GetFeaturesCollection(OtherOptions.Exterior, nameof(OtherOptions.Exterior)));
            configurationFeatures.AddRange(IConvertableToDbCar.GetFeaturesCollection(OtherOptions.Interior, nameof(OtherOptions.Interior)));
            configurationFeatures.AddRange(IConvertableToDbCar.GetFeaturesCollection(OtherOptions.Safety, nameof(OtherOptions.Safety)));

            var carImages = new List<CarImage>();
            foreach (var image in Images)
                carImages.Add(new CarImage { Image = new Image { Url = image } });

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
                },
                ConfigurationFeatures = configurationFeatures
            };

            var dbCarActual = new CarActual
            {
                VinCode = Id,
                Price = int.TryParse(Price, out var price) ? price : null,
                Dealer = new Dealer { Name = dealerName },
                CarImages = carImages,
                InteriorColor = new Color { Name = Colors.Interior },
                ExteriorColor = new Color { Name = Colors.Exterior },
                Configuration = dbConfiguration
            };

            return dbCarActual;
        }
    }
}