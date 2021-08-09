using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;

namespace ApiDPSystem.Models.Parser
{
    //public class CsvParser<T> : IParser<T> where T : FileFormat.IConvertableToDBCar
    //{
    //    private readonly CsvConfiguration config;

    //    public CsvParser()
    //    {
    //        config = new CsvConfiguration(CultureInfo.InvariantCulture)
    //        {
    //            //MissingFieldFound = null,
    //            //HeaderValidated = null
    //        };
    //    }


    //    public Root<T> DeserializeFile(string fileContent)
    //    {
    //        var root = new Root<T>();

    //        using (var reader = new StringReader(fileContent))
    //        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))

    //            root.Cars.AddRange(csv.GetRecords<T>());

    //        return root;
    //    }

    //    public Root<FileFormat.Csv.Version1.Car> DeserializeFile_V1(string fileContent)
    //    {
    //        var root = new Root<FileFormat.Csv.Version1.Car>();

    //        using var reader = new StringReader(fileContent);
    //        using var csv = new CsvReader(reader, config);

    //        while (csv.Read())
    //        {
    //            var car = csv.GetRecord<FileFormat.Csv.Version1.Car>();

    //            var headers = csv.HeaderRecord.ToList();

    //            var images_FieldNames = headers.Where(p => p.StartsWith("images/")).ToList();
    //            car.Images = images_FieldNames.Select(name => csv.GetField(name)).ToList();

    //            var otherOptions_Multimedia_FieldNames = headers.Where(p => p.StartsWith("other options/multimedia/")).ToList();
    //            car.OtherOptions.Multimedia = otherOptions_Multimedia_FieldNames.Select(name => csv.GetField(name)).ToList();

    //            var otherOptions_Interior_FieldNames = headers.Where(p => p.StartsWith("other options/interior/")).ToList();
    //            car.OtherOptions.Interior = otherOptions_Interior_FieldNames.Select(name => csv.GetField(name)).ToList();

    //            var otherOptions_Exterior_FieldNames = headers.Where(p => p.StartsWith("other options/exterior/")).ToList();
    //            car.OtherOptions.Exterior = otherOptions_Exterior_FieldNames.Select(name => csv.GetField(name)).ToList();

    //            root.Cars.Add(car);
    //        }
    //        return root;
    //    }

    //    public Root<FileFormat.Csv.Version2.Car> DeserializeFile_V2(string fileContent)
    //    {
    //        var root = new Root<FileFormat.Csv.Version2.Car>();

    //        using var reader = new StringReader(fileContent);
    //        using var csv = new CsvReader(reader, config);

    //        while (csv.Read())
    //        {
    //            var car = csv.GetRecord<FileFormat.Csv.Version2.Car>();

    //            var headers = csv.HeaderRecord.ToList();

    //            var images_FieldNames = headers.Where(p => p.StartsWith("images/")).ToList();
    //            car.Images = images_FieldNames.Select(name => csv.GetField(name)).ToList();

    //            var otherOptions_Safety_FieldNames = headers.Where(p => p.StartsWith("other options/safety/")).ToList();
    //            car.OtherOptions.Safety = otherOptions_Safety_FieldNames.Select(name => csv.GetField(name)).ToList();

    //            var otherOptions_Exterior_FieldNames = headers.Where(p => p.StartsWith("other options/exterior/")).ToList();
    //            car.OtherOptions.Exterior = otherOptions_Exterior_FieldNames.Select(name => csv.GetField(name)).ToList();

    //            root.Cars.Add(car);
    //        }
    //        return root;
    //    }

    //    public List<Entities.CarActual> MapToDBModel(Root<T> deserializedModels, string dealer)
    //    {
    //        var dbCars = new List<Entities.CarActual>();

    //        foreach (var deserializeModel in deserializedModels.Cars)
    //            dbCars.Add(deserializeModel.ConvertToCarActualDbModel(dealer));

    //        return dbCars;
    //    }
    //}

    public class CsvParser : IBParser
    {
        public string ConvertableFileExtension => ".csv";

        public List<CarActual> Parse(string fileContent, string fileName, string dealer)
        {
            var version = CsvGetVersion(fileName);
            var deserializedType = Selector.GetResultType(ConvertableFileExtension, version.Value);

            var instance = Activator.CreateInstance(deserializedType) as ICsvRoot;

            var deserializedModels = instance.Deserialize(fileContent);

            var dbCars = deserializedModels.ConvertToActualDbModel(dealer);

            return dbCars;
        }

        public Version CsvGetVersion(string fileName)
        {
            var regex = new Regex(@"_v(\d)+\.");
            var matches = regex.Matches(fileName);

            if (matches.Count > 0)
            {
                var versionString = matches.Last().Value.Replace("_v", "").Replace(".", "");
                return new Version {Value = Convert.ToInt32(versionString)};
            }

            return new Version();
        }
    }
}