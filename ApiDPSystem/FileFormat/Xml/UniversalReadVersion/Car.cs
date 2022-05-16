using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ApiDPSystem.FileFormat.Xml.UniversalReadVersion
{
    [XmlRoot(ElementName = "cars")]
    public record Car
    {
        [XmlElement(ElementName = "is_actual")]
        public bool IsActual { get; set; }

        [XmlElement(ElementName = "is_sold")]
        public bool IsSold { get; set; }

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
        public TechnicalOptions TechnicalOptions { get; set; }

        [XmlElement(ElementName = "other_options")]
        public OtherOptions OtherOptions { get; set; }

        [XmlElement(ElementName = "colors")]
        public Colors Colors { get; set; }

        [XmlElement(ElementName = "images")]
        public List<string> Images { get; set; }

        [XmlElement(ElementName = "price")]
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