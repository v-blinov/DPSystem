using System;
using System.Collections.Generic;
using System.Linq;
using ApiDPSystem.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ApiDPSystem.Data
{
    public class ContextPreparation
    {
        public static void PreparationCarsInfo(IApplicationBuilder app)
        {
            try
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    SeedData(serviceScope.ServiceProvider.GetService<Context>());
                }
            }
            catch (Exception ex)
            {
                Log.Error("Ошибка при заполнении тестовых данных о машинах", ex);
            }
        }

        private static void SeedData(Context context)
        {
            if (context.Cars.Any())
            {
                Console.WriteLine("--> We already have cars data");
                return;
            }

            Console.WriteLine("--> Seeding cars data...");

            var testCar = new Car
            {
                VinCode = "ABCDEFGH",
                Dealer = new Dealer { Name = "DefaultDealer" },
                Configuration = new Configuration
                {
                    Year = 2021,
                    Producer = new Producer { Name = "Test" },
                    Model = "TestModel",
                    ModelTrim = "ABC",
                    Engine = new Engine { Fuel = "Benzin", Power = 500, Capacity = 500 },
                    Transmission = "MT",
                    Drive = "AWD"
                },
                CarFeatures = new List<CarFeature>
                {
                    new() { Feature = new Feature { Type = "Safety", Description = "Система контроля слепых зон" } }
                },
                ExteriorColor = new Color { Name = "Marina blue" },
                InteriorColor = new Color { Name = "White" },
                CarImages = new List<CarImage> { new() { Image = new Image { Url = "/test/url.png" } } },
                Price = 1000000,
                Version = 1
            };

            context.Cars.Add(testCar);
            context.SaveChanges();
        }
    }
}