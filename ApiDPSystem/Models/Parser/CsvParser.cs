﻿using ApiDPSystem.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ApiDPSystem.Models.Parser
{
    public class CsvParser<T> : IParser<T> where T : FileFormat.ICar
    {
        private readonly CsvConfiguration config;

        public CsvParser()
        {
            config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                //MissingFieldFound = null,
                //HeaderValidated = null
            };
        }


        public Root<T> DeserializeFile(string fileContent)
        {
            var root = new Root<T>();

            using (var reader = new StringReader(fileContent))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))

                root.Cars.AddRange(csv.GetRecords<T>());

            return root;
        }

        public Root<FileFormat.Csv.Version1.Car> DeserializeFile_V1(string fileContent)
        {
            var root = new Root<FileFormat.Csv.Version1.Car>();

            using var reader = new StringReader(fileContent);
            using var csv = new CsvReader(reader, config);

            while (csv.Read())
            {
                var car = csv.GetRecord<FileFormat.Csv.Version1.Car>();

                var headers = csv.HeaderRecord.ToList();

                var images_FieldNames = headers.Where(p => p.StartsWith("images/")).ToList();
                car.Images = images_FieldNames.Select(name => csv.GetField(name)).ToList();

                var otherOptions_Multimedia_FieldNames = headers.Where(p => p.StartsWith("other options/multimedia/")).ToList();
                car.OtherOptions.Multimedia = otherOptions_Multimedia_FieldNames.Select(name => csv.GetField(name)).ToList();

                var otherOptions_Interior_FieldNames = headers.Where(p => p.StartsWith("other options/interior/")).ToList();
                car.OtherOptions.Interior = otherOptions_Interior_FieldNames.Select(name => csv.GetField(name)).ToList();

                var otherOptions_Exterior_FieldNames = headers.Where(p => p.StartsWith("other options/exterior/")).ToList();
                car.OtherOptions.Exterior = otherOptions_Exterior_FieldNames.Select(name => csv.GetField(name)).ToList();

                root.Cars.Add(car);
            }
            return root;
        }

        public Root<FileFormat.Csv.Version2.Car> DeserializeFile_V2(string fileContent)
        {
            var root = new Root<FileFormat.Csv.Version2.Car>();

            using var reader = new StringReader(fileContent);
            using var csv = new CsvReader(reader, config);

            while (csv.Read())
            {
                var car = csv.GetRecord<FileFormat.Csv.Version2.Car>();

                var headers = csv.HeaderRecord.ToList();

                var images_FieldNames = headers.Where(p => p.StartsWith("images/")).ToList();
                car.Images = images_FieldNames.Select(name => csv.GetField(name)).ToList();

                var otherOptions_Safety_FieldNames = headers.Where(p => p.StartsWith("other options/safety/")).ToList();
                car.OtherOptions.Safety = otherOptions_Safety_FieldNames.Select(name => csv.GetField(name)).ToList();

                var otherOptions_Exterior_FieldNames = headers.Where(p => p.StartsWith("other options/exterior/")).ToList();
                car.OtherOptions.Exterior = otherOptions_Exterior_FieldNames.Select(name => csv.GetField(name)).ToList();

                root.Cars.Add(car);
            }
            return root;
        }

        public List<Entities.CarActual> MapToDBModel(Root<T> deserializedModels, string dealer)
        {
            var dbCars = new List<Entities.CarActual>();

            foreach (var deserializeModel in deserializedModels.Cars)
                dbCars.Add(deserializeModel.ConvertToCarActualDbModel(dealer));

            return dbCars;
        }
    }
}
