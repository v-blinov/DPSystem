using System.Collections.Generic;
using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;
using CsvHelper.Configuration.Attributes;

namespace ApiDPSystem.FileFormat.Csv.Version1
{
    public class Car : IConvertableToDbCar
    {
        public Car()
        {
            OtherOptions = new OtherOptions();
        }

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

        public CarActual ConvertToCarActualDbModel(string dealerName)
        {
            var configurationFeatures = new List<ConfigurationFeature>();

            configurationFeatures.AddRange(IConvertableToDbCar.GetFeaturesCollection(OtherOptions.Exterior, nameof(OtherOptions.Exterior)));
            configurationFeatures.AddRange(IConvertableToDbCar.GetFeaturesCollection(OtherOptions.Interior, nameof(OtherOptions.Interior)));
            configurationFeatures.AddRange(IConvertableToDbCar.GetFeaturesCollection(OtherOptions.Multimedia, nameof(OtherOptions.Multimedia)));

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