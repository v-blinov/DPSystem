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

        public Entities.Car ConvertToDbModel()
        {
            var carFeatures = new List<Entities.CarFeature>();

            carFeatures.AddRange(ICar.GetFeaturesCollection(OtherOptions.Exterior, nameof(OtherOptions.Exterior)));
            carFeatures.AddRange(ICar.GetFeaturesCollection(OtherOptions.Interior, nameof(OtherOptions.Interior)));
            carFeatures.AddRange(ICar.GetFeaturesCollection(OtherOptions.Safety, nameof(OtherOptions.Safety)));

            var carImages = new List<Entities.Image>();
            foreach (var image in Images)
                carImages.Add(new Entities.Image { Url = image });


            var dbCar = new Entities.Car
            {
                VinCode = Id,
                Year = Convert.ToInt32(Year),
                Model = Model,
                ModelTrim = ModelTrim,
                Price = Decimal.TryParse(Price, out decimal price) ? price : null,
                Drive = TechincalOptions.Drive,
                Images = carImages,
                Transmission = new Entities.Transmission { Value = TechincalOptions.Transmission },
                Producer = new Entities.Producer { Name = Make },
                Engine = new Entities.Engine
                {
                    Power = Int32.TryParse(TechincalOptions.Engine.Power, out int power) ? power : null,
                    Fuel = TechincalOptions.Engine.Fuel,
                    Capacity = Double.TryParse(TechincalOptions.Engine.Capacity, out double capacity) ? capacity : null
                },
                InteriorColor = new Entities.Color { Name = Colors.Interior },
                ExteriorColor = new Entities.Color { Name = Colors.Exterior },
                CarFeatures = carFeatures
            };

            return dbCar;
        }
    }
}
