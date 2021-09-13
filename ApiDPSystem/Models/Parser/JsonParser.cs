using ApiDPSystem.Entities;
using ApiDPSystem.Exceptions;
using ApiDPSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ApiDPSystem.Models.Parser
{
    public class JsonParser : IParser
    {
        public string ConvertableFileExtension => ".json";

        public List<Car> Parse(string fileContent, string fileName, string dealer)
        {
            var version = GetVersion(fileContent);
            var deserializedType = Selector.GetResultType(ConvertableFileExtension, version.Value);

            IRoot deserializedModels;
            try
            {
                deserializedModels = JsonSerializer.Deserialize(fileContent, deserializedType) as IRoot;
            }
            catch (Exception ex)
            {
                throw new InvalidFileException("Невозможно обработать содержимое файла", ex);
            }

            var dbCars = deserializedModels!.ConvertToActualDbModel(dealer);
            return dbCars;
        }

        private Version GetVersion(string file)
        {
            try
            {
                return JsonSerializer.Deserialize<Version>(file);
            }
            catch (Exception ex)
            {
                throw new InvalidFileException("Невозможно обработать содержимое файла", ex);
            }
        }
    }
}