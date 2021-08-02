using ApiDPSystem.Entities;
using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.Version1
{
    //public class Car : ICar
    //{
    //    [YamlMember(Alias = "id")]
    //    public string Id { get; set; }

    //    [YamlMember(Alias = "year")]
    //    public string Year { get; set; }

    //    [YamlMember(Alias = "make")]
    //    public string Make { get; set; }

    //    [YamlMember(Alias = "model")]
    //    public string Model { get; set; }

    //    [YamlMember(Alias = "model trim")]
    //    public string ModelTrim { get; set; }

    //    [YamlMember(Alias = "techincal options")]
    //    public TechincalOptions TechincalOptions { get; set; }

    //    [YamlMember(Alias = "other options")]
    //    public OtherOptions OtherOptions { get; set; }

    //    [YamlMember(Alias = "colors")]
    //    public Colors Colors { get; set; }

    //    [YamlMember(Alias = "images")]
    //    public List<string> Images { get; set; }

    //    [YamlMember(Alias = "price")]
    //    public string Price { get; set; }

    //    public Entities.Configuration ConvertToDbModel(string DealerName)
    //    {
    //        var carFeatures = new List<Entities.ConfigurationFeature>();

    //        carFeatures.AddRange(ICar.GetFeaturesCollection(OtherOptions.Exterior, nameof(OtherOptions.Exterior)));
    //        carFeatures.AddRange(ICar.GetFeaturesCollection(OtherOptions.Interior, nameof(OtherOptions.Interior)));
    //        carFeatures.AddRange(ICar.GetFeaturesCollection(OtherOptions.Safety, nameof(OtherOptions.Safety)));

    //        var carImages = new List<Entities.Image>();
    //        foreach (var image in Images)
    //            carImages.Add(new Entities.Image { Url = image });

    //        var carEntities = new List<CarEntity>();
    //        carEntities.Add(new CarEntity
    //        {
    //            VinCode = Id,
    //            Dealer = new Dealer { Name = DealerName },
    //            Price = Decimal.TryParse(Price, out decimal price) ? price : null,
    //            IsSold = false,
    //        });

    //        var dbCar = new Entities.Configuration
    //        {
    //            CarEntities = carEntities,
    //            Year = Convert.ToInt32(Year),
    //            Model = Model,
    //            ModelTrim = ModelTrim,
    //            Drive = TechincalOptions.Drive,
    //            Images = carImages,
    //            Transmission = new Entities.Transmission { Value = TechincalOptions.Transmission },
    //            Producer = new Entities.Producer { Name = Make },
    //            Engine = new Entities.Engine
    //            {
    //                Power = Int32.TryParse(TechincalOptions.Engine.Power, out int power) ? power : null,
    //                Fuel = TechincalOptions.Engine.Fuel,
    //                Capacity = Double.TryParse(TechincalOptions.Engine.Capacity, out double capacity) ? capacity : null
    //            },
    //            InteriorColor = new Entities.Color { Name = Colors.Interior },
    //            ExteriorColor = new Entities.Color { Name = Colors.Exterior },
    //            ConfigurationFeatures = carFeatures
    //        };

    //        return dbCar;
    //    }
    //}
}
