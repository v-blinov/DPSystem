﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ApiDPSystem.Entities;
using ApiDPSystem.Exceptions;
using ApiDPSystem.Interfaces;

namespace ApiDPSystem.Models.Parser
{
    public class CsvParser : IParser
    {
        public string ConvertableFileExtension => ".csv";

        public List<Car> Parse(string fileContent, string fileName, string dealer)
        {
            var version = GetVersion(fileName);
            var deserializedType = Selector.GetResultType(ConvertableFileExtension, version.Value);

            var instance = Activator.CreateInstance(deserializedType) as ICsvRoot;

            IRoot deserializedModels;
            try
            {
                deserializedModels = instance.Deserialize(fileContent);
            }
            catch (Exception ex)
            {
                throw new InvalidFileException("Невозможно обработать содержимое файла", ex);
            }

            var dbCars = deserializedModels.ConvertToActualDbModel(dealer);

            return dbCars;
        }

        public Version GetVersion(string fileName)
        {
            var regex = new Regex(@"_v(\d)+\.");
            var matches = regex.Matches(fileName);

            if (matches.Count > 0)
            {
                var versionString = matches.Last().Value.Replace("_v", "").Replace(".", "");
                return new Version { Value = Convert.ToInt32(versionString) };
            }

            return new Version();
        }
    }
}