using ApiDPSystem.Interfaces;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ApiDPSystem.Models.Parser
{
    public class CsvParser<T> : IParser<T> where T : FileFormat.Csv.Version1.Car
    {
        public Root<T> DeserializeFile(string fileContent)
        {
            var root = new Root<T>()
            {
                Cars = new List<T>()
            };

            // вынести десериализацию общих полей в отдельный метод
            using (var reader = new StringReader(fileContent))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {

                    while (csv.Read())
                    {
                        var car = csv.GetRecord<T>();

                        // Вынести инициализацию листов и объектов в конструкторы классов
                        car.Images = new List<string>();
                        car.OtherOptions = new FileFormat.Csv.Version1.OtherOptions();

                        car.OtherOptions.Multimedia = new List<string>();
                        car.OtherOptions.Exterior = new List<string>();
                        car.OtherOptions.Interior = new List<string>();

                        var headers = csv.HeaderRecord.ToList();

                        var image_FieldNames = headers.Where(p => p.StartsWith("images/")).ToList();
                        var otherOptions_Multimedia_FieldNames = headers.Where(p => p.StartsWith("other options/multimedia/")).ToList();
                        var otherOptions_Interior_FieldNames = headers.Where(p => p.StartsWith("other options/interior/")).ToList();
                        var otherOptions_Exterior_FieldNames = headers.Where(p => p.StartsWith("other options/exterior/")).ToList();

                        foreach (var name in image_FieldNames)
                            car.Images.Add(csv.GetField(name));

                        foreach (var name in otherOptions_Multimedia_FieldNames)
                            car.OtherOptions.Multimedia.Add(csv.GetField(name));

                        foreach (var name in otherOptions_Interior_FieldNames)
                            car.OtherOptions.Interior.Add(csv.GetField(name));

                        foreach (var name in otherOptions_Exterior_FieldNames)
                            car.OtherOptions.Exterior.Add(csv.GetField(name));

                        root.Cars.Add(car);
                    }
                }
            }
            return root;
        }

        public void SetDataToDatabase(Root<T> data)
        {
            throw new System.NotImplementedException();
        }
    }
}
