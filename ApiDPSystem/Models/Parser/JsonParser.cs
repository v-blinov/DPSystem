using ApiDPSystem.Interfaces;
using System.Collections.Generic;
using System.Text.Json;

namespace ApiDPSystem.Models.Parser
{
    public class JsonParser<T> : IParser<T> where T : FileFormat.ICar
    {
        public Root<T> DeserializeFile(string fileContent) =>
            JsonSerializer.Deserialize<Root<T>>(fileContent);
        //public List<Entities.CarConfiguration> MapToDBModel(Root<T> deserializedModels)
        //{


        //    var dbCars = new List<Entities.CarConfiguration>();

        //    foreach (var deserializeModel in deserializedModels.Cars)
        //        dbCars.Add(deserializeModel.ConvertToDbModel());

        //    return dbCars;
        //}
        //public void MapToDbModel_V1(FileFormat.Json.Version1.Car deserializedCar)
        //{
        //    //var dbCar = new Entities.Car
        //    //{
        //    //    VinCode = deserializedCar.Id,
        //    //    Year = Convert.ToInt32(deserializedCar.Year),
        //    //    Model = deserializedCar.Model,
        //    //    ModelTrim = deserializedCar.ModelTrim,
        //    //    Price = Decimal.TryParse(deserializedCar.Price, out decimal price) ? price : null,
        //    //    Drive = deserializedCar.TechincalOptions.Drive,
        //    //    //Images = (deserializedCar.Images).Select(p => new Entities.Image { Url = p }).ToList()
        //    //};

        //    //var storedTransmission = _mapperRepository.GetTransmission(deserializedCar.TechincalOptions.Transmission);
        //    //if (storedTransmission is null)
        //    //    dbCar.Transmission = new Entities.Transmission { Value = deserializedCar.TechincalOptions.Transmission };
        //    //else
        //    //    dbCar.TransmissionId = storedTransmission.Id;


        //    //var storedProducer = _mapperRepository.GetProducer(deserializedCar.Make);
        //    //if (storedProducer is null)
        //    //    dbCar.Producer = new Entities.Producer { Name = deserializedCar.Make };
        //    //else
        //    //    dbCar.ProducerId = storedProducer.Id;


        //    //var currentEngine = new Entities.Engine
        //    //{
        //    //    Power = Int32.TryParse(deserializedCar.TechincalOptions.Engine.Power, out int power) ? power : null,
        //    //    Fuel = deserializedCar.TechincalOptions.Engine.Fuel,
        //    //    Capacity = Double.TryParse(deserializedCar.TechincalOptions.Engine.Capacity, out double capacity) ? capacity : null
        //    //};
        //    //var storedEngine = _mapperRepository.GetEngine(currentEngine);
        //    //if (storedEngine is null)
        //    //    dbCar.Engine = currentEngine;
        //    //else
        //    //    dbCar.EngineId = storedEngine.Id;


        //    //var storedInteriorColor = _mapperRepository.getColor(deserializedCar.Colors.Interior);
        //    //if (storedInteriorColor is null)
        //    //    dbCar.InteriorColor = new Entities.Color { Name = deserializedCar.Colors.Interior };
        //    //else
        //    //    dbCar.InteriorColor.Id = storedInteriorColor.Id;

        //    /////Изменить логику добавления цвета, т.к. сейчас есть баг при добавлении нового одинакогого цвета для интерьера и экстерьера
        //    //var storedExteriorColor = _mapperRepository.getColor(deserializedCar.Colors.Exterior);
        //    //if (storedExteriorColor is null)
        //    //    dbCar.InteriorColor = new Entities.Color { Name = deserializedCar.Colors.Exterior };
        //    //else
        //    //    dbCar.InteriorColor.Id = storedExteriorColor.Id;



        //    //dbCar.CarFeatures.


        //    //var newFeatures = new List<Entities.Feature>();


        //    //var featureType = _mapperRepository.GetFeatureType("Exterior");
        //    //foreach (var featureDescription in deserializedCar.OtherOptions.Exterior)
        //    //{
        //    //    if (featureType == null)
        //    //    {
        //    //        var feature = _mapperRepository.GetFeature()
        //    //    }
        //    //    else
        //    //    { 

        //    //    }

        //    //}




        //    //    #region
        //    ////    var car = new DbEntities.Car();

        //    ////    foreach (var model in deserializeModel.Cars)
        //    ////    {
        //    ////        car.VinCode = model.Id;
        //    ////        car.Year = Convert.ToInt32(model.Year);
        //    ////        car.Model = model.Model;
        //    ////        car.ModelTrim = model.ModelTrim;
        //    ////        car.Price = Convert.ToDecimal(model.Price);
        //    ////        car.Producer.Name = model.Make;
        //    ////        car.Engine.Fuel = model.TechincalOptions.Engine.Fuel;
        //    ////        car.Transmission.Value = model.TechincalOptions.Transmission;
        //    ////        car.Drive = model.TechincalOptions.Drive;

        //    ////        if (model.TechincalOptions.Engine.Power != null)
        //    ////            car.Engine.Power = Convert.ToInt32(model.TechincalOptions.Engine.Power);

        //    ////        if (model.TechincalOptions.Engine.Capacity != null)
        //    ////            car.Engine.Capacity = Convert.ToDouble(model.TechincalOptions.Engine.Capacity);

        //    ////        if (model.Price != null)
        //    ////            car.Price = Convert.ToDecimal(model.Price);

        //    ////        foreach (var interior in model.OtherOptions.Interior)
        //    ////            car.CarFeature.Features.

        //    ////    }
        //    //    #endregion  
        //}


    }
}
