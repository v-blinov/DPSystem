using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ApiDPSystem.Entities;
using ApiDPSystem.Interfaces;

namespace ApiDPSystem.Models.Parser
{
    //public class XmlParser<T> : IParser<T> where T : IConvertableToDBCar
    //{
    //    public Root<T> DeserializeFile(string fileContent)
    //    {
    //        var serializer = new XmlSerializer(typeof(Root<T>));

    //        using var reader = new StringReader(fileContent);
    //        return (Root<T>)serializer.Deserialize(reader);
    //    }

    //    public List<CarActual> MapToDBModel(Root<T> deserializedModels, string dealer)
    //    {
    //        var dbCarActuals = new List<CarActual>();

    //        foreach (var deserializeModel in deserializedModels.Cars)
    //            dbCarActuals.Add(deserializeModel.ConvertToCarActualDbModel(dealer));

    //        return dbCarActuals;
    //    }
    //}

    public class XmlParser : IBParser
    {
        public string ConvertableFileExtension => ".xml";

        public List<CarActual> Parse(string fileContent, string fileName, string dealer)
        {
            var version = XmlGetVersion(fileContent);
            var deserializedType = Selector.GetResultType(ConvertableFileExtension, version.Value);


            var serializer = new XmlSerializer(deserializedType);

            using var reader = new StringReader(fileContent);
            var deserializedModels = serializer.Deserialize(reader) as IRoot;

            var dbCars = deserializedModels.ConvertToActualDbModel(dealer);
            return dbCars;
        }

        public Version XmlGetVersion(string fileContent)
        {
            var serializer = new XmlSerializer(typeof(Version));

            using var reader = new StringReader(fileContent);
            return (Version) serializer.Deserialize(reader);
        }
    }
}