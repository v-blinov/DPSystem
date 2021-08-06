using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.Version1
{
    public class Car : ICar
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
        public TechincalOptions TechincalOptions { get; set; }

        [YamlMember(Alias = "other options")]
        public OtherOptions OtherOptions { get; set; }

        [YamlMember(Alias = "colors")]
        public Colors Colors { get; set; }

        [YamlMember(Alias = "images")]
        public List<string> Images { get; set; }

        [YamlMember(Alias = "price")]
        public string Price { get; set; }

        public Entities.CarActual ConvertToCarActualDbModel(string DealerName)
        {
            var configurationFeatures = new List<Entities.ConfigurationFeature>();

            configurationFeatures.AddRange(ICar.GetFeaturesCollection(OtherOptions.Exterior, nameof(OtherOptions.Exterior)));
            configurationFeatures.AddRange(ICar.GetFeaturesCollection(OtherOptions.Interior, nameof(OtherOptions.Interior)));
            configurationFeatures.AddRange(ICar.GetFeaturesCollection(OtherOptions.Safety, nameof(OtherOptions.Safety)));

            var carImages = new List<Entities.CarImage>();
            foreach (var image in Images)
                carImages.Add(new Entities.CarImage { Image = new Entities.Image { Url = image } });

            var dbConfiguration = new Entities.Configuration
            {
                Year = Int32.Parse(Year),
                Model = Model,
                ModelTrim = ModelTrim,
                Transmission = TechincalOptions.Transmission,
                Drive = TechincalOptions.Drive,
                Producer = new Entities.Producer { Name = Make },
                Engine = new Entities.Engine
                {
                    Power = Int32.TryParse(TechincalOptions.Engine.Power, out int power) ? power : null,
                    Fuel = TechincalOptions.Engine.Fuel,
                    Capacity = Double.TryParse(TechincalOptions.Engine.Capacity, out double capacity) ? capacity : null
                },
                ConfigurationFeatures = configurationFeatures
            };

            var dbCarActual = new Entities.CarActual
            {
                VinCode = Id,
                Price = Int32.TryParse(Price, out int price) ? price : null,
                Dealer = new Entities.Dealer { Name = DealerName },
                CarImages = carImages,
                InteriorColor = new Entities.Color { Name = Colors.Interior },
                ExteriorColor = new Entities.Color { Name = Colors.Exterior },
                Configuration = dbConfiguration
            };

            return dbCarActual;
        }
    }
}
