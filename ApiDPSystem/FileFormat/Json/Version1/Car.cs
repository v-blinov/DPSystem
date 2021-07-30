using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiDPSystem.FileFormat.Json.Version1
{
    public class Car : ICar
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
        public TechincalOptions TechincalOptions { get; set; }

        [JsonPropertyName("other options")]
        public OtherOptions OtherOptions { get; set; }

        [JsonPropertyName("colors")]
        public Colors Colors { get; set; }

        [JsonPropertyName("images")]
        public List<string> Images { get; set; }

        [JsonPropertyName("price")]
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

        //private List<Entities.CarFeature> GetFeaturesCollection(List<string> collection, string featureType)
        //{
        //    var features = new List<Entities.CarFeature>();

        //    if (collection != null)
        //    {
        //        foreach (var feature in collection)
        //            features.Add(new Entities.CarFeature { Feature = new Entities.Feature { FeatureType = new Entities.FeatureType { Name = featureType }, Description = feature } });
        //    }

        //    return features;
        //}
    }
}
