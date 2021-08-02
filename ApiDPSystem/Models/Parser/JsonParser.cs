using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;
using System.Collections.Generic;
using System.Text.Json;

namespace ApiDPSystem.Models.Parser
{
    public class JsonParser<T> : IParser<T> where T : FileFormat.ICar
    {
        public Root<T> DeserializeFile(string fileContent) =>
            JsonSerializer.Deserialize<Root<T>>(fileContent);

        public List<CarEntity> MapToDBModel(Root<T> deserializedModels, string dealer)
        {
            var dbCars = new List<CarEntity>();

            foreach (var deserializeModel in deserializedModels.Cars)
                dbCars.Add(deserializeModel.ConvertToCarEntityDbModel(dealer));

            return dbCars;
        }

        //public void maptodbmodel_v1(fileformat.json.version1.car deserializedcar)
        //{
        //    var dbcar = new entities.car
        //    {
        //        vincode = deserializedcar.id,
        //        year = convert.toint32(deserializedcar.year),
        //        model = deserializedcar.model,
        //        modeltrim = deserializedcar.modeltrim,
        //        price = decimal.tryparse(deserializedcar.price, out decimal price) ? price : null,
        //        drive = deserializedcar.techincaloptions.drive,
        //        //images = (deserializedcar.images).select(p => new entities.image { url = p }).tolist()
        //    };

        //    var storedtransmission = _mapperrepository.gettransmission(deserializedcar.techincaloptions.transmission);
        //    if (storedtransmission is null)
        //        dbcar.transmission = new entities.transmission { value = deserializedcar.techincaloptions.transmission };
        //    else
        //        dbcar.transmissionid = storedtransmission.id;


        //    var storedproducer = _mapperrepository.getproducer(deserializedcar.make);
        //    if (storedproducer is null)
        //        dbcar.producer = new entities.producer { name = deserializedcar.make };
        //    else
        //        dbcar.producerid = storedproducer.id;


        //    var currentengine = new entities.engine
        //    {
        //        power = int32.tryparse(deserializedcar.techincaloptions.engine.power, out int power) ? power : null,
        //        fuel = deserializedcar.techincaloptions.engine.fuel,
        //        capacity = double.tryparse(deserializedcar.techincaloptions.engine.capacity, out double capacity) ? capacity : null
        //    };
        //    var storedengine = _mapperrepository.getengine(currentengine);
        //    if (storedengine is null)
        //        dbcar.engine = currentengine;
        //    else
        //        dbcar.engineid = storedengine.id;


        //    var storedinteriorcolor = _mapperrepository.getcolor(deserializedcar.colors.interior);
        //    if (storedinteriorcolor is null)
        //        dbcar.interiorcolor = new entities.color { name = deserializedcar.colors.interior };
        //    else
        //        dbcar.interiorcolor.id = storedinteriorcolor.id;

        //    ///изменить логику добавления цвета, т.к. сейчас есть баг при добавлении нового одинакогого цвета для интерьера и экстерьера
        //    var storedexteriorcolor = _mapperrepository.getcolor(deserializedcar.colors.exterior);
        //    if (storedexteriorcolor is null)
        //        dbcar.interiorcolor = new entities.color { name = deserializedcar.colors.exterior };
        //    else
        //        dbcar.interiorcolor.id = storedexteriorcolor.id;



        //    dbcar.carfeatures.


        //    var newfeatures = new list<entities.feature>();


        //    var featuretype = _mapperrepository.getfeaturetype("exterior");
        //    foreach (var featuredescription in deserializedcar.otheroptions.exterior)
        //    {
        //        if (featuretype == null)
        //        {
        //            var feature = _mapperrepository.getfeature()
        //        }
        //        else
        //        {

        //        }

        //    }




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
