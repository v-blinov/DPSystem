using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;

namespace ApiDPSystem.Models.Parser
{
    public class JsonParser : Parser
    {
        public override void ProcessFile(IFormFile file)
        {
            string jsonContent;

            using (var reader = new StreamReader(file.OpenReadStream()))
                jsonContent = reader.ReadToEnd();
            

            var deserializeModel = JsonSerializer.Deserialize<FileFormat.Json.Version1.Root>(jsonContent);

            //var car = new DbEntities.Car();

            //foreach (var model in deserializeModel.Cars)
            //{
            //    car.VinCode = model.Id;
            //    car.Year = Convert.ToInt32(model.Year);
            //    car.Model = model.Model;
            //    car.ModelTrim = model.ModelTrim;
            //    car.Price = Convert.ToDecimal(model.Price);
            //    car.Producer.Name = model.Make;
            //    car.Engine.Fuel = model.TechincalOptions.Engine.Fuel;
            //    car.Transmission.Value = model.TechincalOptions.Transmission;
            //    car.Drive = model.TechincalOptions.Drive;

            //    if (model.TechincalOptions.Engine.Power != null)
            //        car.Engine.Power = Convert.ToInt32(model.TechincalOptions.Engine.Power);

            //    if (model.TechincalOptions.Engine.Capacity != null)
            //        car.Engine.Capacity = Convert.ToDouble(model.TechincalOptions.Engine.Capacity);

            //    if (model.Price != null)
            //        car.Price = Convert.ToDecimal(model.Price);

            //    foreach (var interior in model.OtherOptions.Interior)
            //        car.CarFeature.Features.

            //}
        }
    }
}
