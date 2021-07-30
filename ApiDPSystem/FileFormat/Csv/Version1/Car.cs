﻿using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;

namespace ApiDPSystem.FileFormat.Csv.Version1
{
    public class Car : ICar
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

        public TechincalOptions TechincalOptions { get; set; }

        public OtherOptions OtherOptions { get; set; }

        public Colors Colors { get; set; }

        public List<string> Images { get; set; }

        [Name("price")]
        public string Price { get; set; }

        public Entities.Car ConvertToDbModel()
        {
            var carFeatures = new List<Entities.CarFeature>();

            carFeatures.AddRange(ICar.GetFeaturesCollection(OtherOptions.Exterior, nameof(OtherOptions.Exterior)));
            carFeatures.AddRange(ICar.GetFeaturesCollection(OtherOptions.Interior, nameof(OtherOptions.Interior)));
            carFeatures.AddRange(ICar.GetFeaturesCollection(OtherOptions.Multimedia, nameof(OtherOptions.Multimedia)));

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
