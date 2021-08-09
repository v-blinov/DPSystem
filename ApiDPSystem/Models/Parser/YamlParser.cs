﻿using System.Collections.Generic;
using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;
using YamlDotNet.Serialization;

namespace ApiDPSystem.Models.Parser
{
    //public class YamlParser<T> : IParser<T> where T : IConvertableToDBCar
    //{
    //    public Root<T> DeserializeFile(string fileContent)
    //    {
    //        var deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
    //        return deserializer.Deserialize<Root<T>>(fileContent);
    //    }

    //    public List<CarActual> MapToDBModel(Root<T> deserializedModels, string dealer)
    //    {
    //        var dbCars = new List<CarActual>();

    //        foreach (var deserializeModel in deserializedModels.Cars)
    //            dbCars.Add(deserializeModel.ConvertToCarActualDbModel(dealer));

    //        return dbCars;
    //    }
    //}


    public class YamlParser : IBParser
    {
        public string ConvertableFileExtension => ".yaml";

        public List<CarActual> Parse(string fileContent, string fileName, string dealer)
        {
            var version = YamlGetVersion(fileContent);
            var deserializedType = Selector.GetResultType(ConvertableFileExtension, version.Value);

            var deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
            var deserializedModels = deserializer.Deserialize(fileContent, deserializedType) as IRoot;

            var dbCars = deserializedModels.ConvertToActualDbModel(dealer);

            return dbCars;
        }

        public Version YamlGetVersion(string fileContent)
        {
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();

            return deserializer.Deserialize<Version>(fileContent);
        }
    }
}