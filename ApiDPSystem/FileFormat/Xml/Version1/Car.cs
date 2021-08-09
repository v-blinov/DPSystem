using System.Collections.Generic;
using System.Xml.Serialization;
using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;

namespace ApiDPSystem.FileFormat.Xml.Version1
{
    [XmlRoot(ElementName = "cars")]
    public class Car : IConvertableToDBCar
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


        public CarActual ConvertToCarActualDbModel(string DealerName)
        {
            var configurationFeatures = new List<ConfigurationFeature>();

            configurationFeatures.AddRange(IConvertableToDBCar.GetFeaturesCollection(OtherOptions.Exterior, nameof(OtherOptions.Exterior)));
            configurationFeatures.AddRange(IConvertableToDBCar.GetFeaturesCollection(OtherOptions.Interior, nameof(OtherOptions.Interior)));
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