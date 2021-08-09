using System.Collections.Generic;
using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;
using CsvHelper.Configuration.Attributes;

namespace ApiDPSystem.FileFormat.Csv.Version2
{
    public class Car : IConvertableToDBCar
    {
        public Car()
        {
            OtherOptions = new OtherOptions();
            Images = new List<string>();
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

        public TechincalOptions TechincalOptions { get; set; }

        public OtherOptions OtherOptions { get; set; }

        public Colors Colors { get; set; }

        public List<string> Images { get; set; }

        [Name("price")]
        public string Price { get; set; }


        public CarActual ConvertToCarActualDbModel(string DealerName)
        {
            var configurationFeatures = new List<ConfigurationFeature>();

            configurationFeatures.AddRange(IConvertableToDBCar.GetFeaturesCollection(OtherOptions.Exterior, nameof(OtherOptions.Exterior)));
            configurationFeatures.AddRange(IConvertableToDBCar.GetFeaturesCollection(OtherOptions.Safety, nameof(OtherOptions.Safety)));

            var carImages = new List<CarImage>();
            foreach (var image in Images)
                carImages.Add(new CarImage {Image = new Image {Url = image}});

            var dbConfiguration = new Configuration
            {
                Year = int.Parse(Year),
                Model = Model,
                ModelTrim = ModelTrim,
                Transmission = TechincalOptions.Transmission,
                Drive = TechincalOptions.Drive,
                Producer = new Producer {Name = Make},
                Engine = new Entities.Engine
                {
                    Power = int.TryParse(TechincalOptions.Engine.Power, out var power) ? power : null,
                    Fuel = TechincalOptions.Engine.Fuel,
                    Capacity = double.TryParse(TechincalOptions.Engine.Capacity, out var capacity) ? capacity : null
                },
                ConfigurationFeatures = configurationFeatures
            };

            var dbCarActual = new CarActual
            {
                VinCode = Id,
                Price = int.TryParse(Price, out var price) ? price : null,
                Dealer = new Dealer {Name = DealerName},
                CarImages = carImages,
                InteriorColor = new Color {Name = Colors.Interior},
                ExteriorColor = new Color {Name = Colors.Exterior},
                Configuration = dbConfiguration
            };

            return dbCarActual;
        }
    }
}