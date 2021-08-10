﻿using System.Collections.Generic;
using System.Text.Json;
using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;

namespace ApiDPSystem.Models.Parser
{
    //public class JsonParser<T> : IParser<T> where T : FileFormat.ICar
    //{
    //    public Root<T> DeserializeFile(string fileContent) =>
    //        JsonSerializer.Deserialize<Root<T>>(fileContent);

    //    public List<CarActual> MapToDBModel(Root<T> deserializedModels, string dealer)
    //    {
    //        var dbCars = new List<CarActual>();

    //        foreach (var deserializeModel in deserializedModels.Cars)
    //            dbCars.Add(deserializeModel.ConvertToCarActualDbModel(dealer));

    //        return dbCars;
    //    }
    //}

    public class JsonParser : IBParser
    {
        public string ConvertableFileExtension => ".json";

        public List<CarActual> Parse(string fileContent, string fileName, string dealer)
        {
            var version = JsonGetVersion(fileContent);
            var deserializedType = Selector.GetResultType(ConvertableFileExtension, version.Value);

            var deserializedModels = JsonSerializer.Deserialize(fileContent, deserializedType) as IRoot;

            var dbCars = deserializedModels!.ConvertToActualDbModel(dealer);

            return dbCars;
        }

        private Version JsonGetVersion(string file)
        {
            return JsonSerializer.Deserialize<Version>(file);
        }
    }
}