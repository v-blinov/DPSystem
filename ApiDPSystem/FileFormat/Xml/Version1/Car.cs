using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ApiDPSystem.FileFormat.Xml.Version1
{
    [XmlRoot(ElementName = "cars")]
    public class Car : ICar
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "year")]
        public string Year { get; set; }

        [XmlElement(ElementName = "make")]
        public string Make { get; set; }

        [XmlElement(ElementName = "model")]
        public string Model { get; set; }

        [XmlElement(ElementName = "model_trim")]
        public string ModelTrim { get; set; }

        [XmlElement(ElementName = "techincal_options")]
        public TechincalOptions TechincalOptions { get; set; }

        [XmlElement(ElementName = "other_options")]
        public OtherOptions OtherOptions { get; set; }

        [XmlElement(ElementName = "colors")]
        public Colors Colors { get; set; }

        [XmlElement(ElementName = "images")]
        public List<string> Images { get; set; }

        [XmlElement(ElementName = "price")]
        public string Price { get; set; }


        public Entities.CarEntity ConvertToCarEntityDbModel(string DealerName)
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

            var dbCarEntity = new Entities.CarEntity
            {
                VinCode = Id,
                Price = Int32.TryParse(Price, out int price) ? price : null,
                Dealer = new Entities.Dealer { Name = DealerName },
                CarImages = carImages,
                InteriorColor = new Entities.Color { Name = Colors.Interior },
                ExteriorColor = new Entities.Color { Name = Colors.Exterior },
                Configuration = dbConfiguration
            };

            return dbCarEntity;
        }
    }
}
