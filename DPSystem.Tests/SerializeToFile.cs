using System.Collections.Generic;
using ApiDPSystem.Data;
using ApiDPSystem.Entities;
using ApiDPSystem.Models;
using ApiDPSystem.Repository;
using ApiDPSystem.Repository.Interfaces;
using ApiDPSystem.Services;
using ApiDPSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DPSystem.Tests
{
    public class SerializeToFile
    {
        private const string DefaultDealer = "DefaultDealer";
        private const string TestConnectionString = "Server=mssql,8083;Database=DPSystem.Tests;User=sa;Password=Qwerty123!;";
        private readonly DbContextOptions<Context> _testContextOptions = new DbContextOptionsBuilder<Context>().UseSqlServer(TestConnectionString).Options;

        private readonly Car _defaultDbCar = new Car
        {
            VinCode = "ABCDEFGH",
            Dealer = new Dealer { Name = DefaultDealer },
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
            Price = 1000000
        };

        private Car CreateDefaultCarWithNewVincode(string vincode)
        {
            var copy = _defaultDbCar.Copy();
            copy.VinCode = vincode;
            return copy;
        }
        private void CreateDatabase()
        {
            using var context = new Context(_testContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }


        [InlineData("DefaultDealer", 3, 3)]
        [InlineData("selectedDealer", 3, 2)]
        [Theory]
        public void Check_filter_select_for_dealer(string dealerName, int category, int expectedCount)
        {
            //Arrange
            var filter = new Filter
            {
                DealerName = dealerName,
                Category = (Category)category,
                FileFormat = FileFormat.unknown
            };
            var secondDealer = new Dealer() { Name = "selectedDealer" };
            var existedCars = new List<Car>
            {
                CreateDefaultCarWithNewVincode("1_dealer_Actual_1"),
                CreateDefaultCarWithNewVincode("1_dealer_Actual_2"),
                CreateDefaultCarWithNewVincode("1_dealer_Actual_3"),
                CreateDefaultCarWithNewVincode("2_dealer_Actual_1"),
                CreateDefaultCarWithNewVincode("2_dealer_Actual_2"),
                CreateDefaultCarWithNewVincode("1_dealer_Sold_1"),
                CreateDefaultCarWithNewVincode("1_dealer_Sold_2"),
                CreateDefaultCarWithNewVincode("2_dealer_Sold_1"),
                CreateDefaultCarWithNewVincode("2_dealer_Sold_2"),
                CreateDefaultCarWithNewVincode("2_dealer_Sold_3")
            };
            foreach (var car in existedCars)
            {
                if (car.VinCode.Contains("_Actual_"))
                {
                    car.IsActual = true;
                    car.IsSold = false;
                }
                else if (car.VinCode.Contains("_Sold_"))
                {
                    car.IsActual = false;
                    car.IsSold = true;
                }

                if (car.VinCode.Contains("2_dealer"))
                    car.Dealer = secondDealer;
            }


            CreateDatabase();
            using (var context = new Context(_testContextOptions))
            {
                context.AddRange(existedCars);
                context.SaveChanges();
            }

            //Act
            List<Car> resultActuals;
            using (var context = new Context(_testContextOptions))
            {
                var sut = new CarRepository(context);
                resultActuals = sut.GetFullCarsInfoWithFilter(filter);
            }

            //Assert
            Assert.Equal(expectedCount, resultActuals.Count);
            foreach (var car in resultActuals)
            {
                Assert.Equal(dealerName, car.Dealer.Name);
                Assert.True(car.IsActual);
                Assert.False(car.IsSold);

            }
        }

        
        [InlineData("DefaultDealer", 2, 2)]
        [InlineData("selectedDealer", 2, 3)]
        [Theory]
        public void Check_filter_select_only_sold_for_dealer(string dealerName, int category, int expectedCount)
        {
            //Arrange
            var filter = new Filter
            {
                DealerName = dealerName,
                Category = (Category)category,
                FileFormat = FileFormat.unknown
            };
            var secondDealer = new Dealer() { Name = "selectedDealer" };
            var existedCars = new List<Car>
            {
                CreateDefaultCarWithNewVincode("1_dealer_Actual_1"),
                CreateDefaultCarWithNewVincode("1_dealer_Actual_2"),
                CreateDefaultCarWithNewVincode("1_dealer_Actual_3"),
                CreateDefaultCarWithNewVincode("2_dealer_Actual_1"),
                CreateDefaultCarWithNewVincode("2_dealer_Actual_2"),
                CreateDefaultCarWithNewVincode("1_dealer_Sold_1"),
                CreateDefaultCarWithNewVincode("1_dealer_Sold_2"),
                CreateDefaultCarWithNewVincode("2_dealer_Sold_1"),
                CreateDefaultCarWithNewVincode("2_dealer_Sold_2"),
                CreateDefaultCarWithNewVincode("2_dealer_Sold_3")
            };
            foreach (var car in existedCars)
            {
                if (car.VinCode.Contains("_Actual_"))
                {
                    car.IsActual = true;
                    car.IsSold = false;
                }
                else if (car.VinCode.Contains("_Sold_"))
                {
                    car.IsActual = false;
                    car.IsSold = true;
                }

                if (car.VinCode.Contains("2_dealer"))
                    car.Dealer = secondDealer;
            }


            CreateDatabase();
            using (var context = new Context(_testContextOptions))
            {
                context.AddRange(existedCars);
                context.SaveChanges();
            }

            //Act
            List<Car> resultActuals;
            using (var context = new Context(_testContextOptions))
            {
                var sut = new CarRepository(context);
                resultActuals = sut.GetFullCarsInfoWithFilter(filter);
            }

            //Assert
            Assert.Equal(expectedCount, resultActuals.Count);
            foreach (var car in resultActuals)
            {
                Assert.Equal(dealerName, car.Dealer.Name);
                Assert.True(car.IsSold);
                Assert.False(car.IsActual);
            }
        }

        
        [InlineData("DefaultDealer", 0, 5)]
        [InlineData("selectedDealer", 0, 5)]
        [Theory]
        public void Check_filter_select_when_filter_disabled(string dealerName, int category, int expectedCount)
        {
            //Arrange
            var filter = new Filter
            {
                DealerName = dealerName,
                Category = (Category)category,
                FileFormat = FileFormat.unknown
            };
            var secondDealer = new Dealer() { Name = "selectedDealer" };
            var existedCars = new List<Car>
            {
                CreateDefaultCarWithNewVincode("1_dealer_Actual_1"),
                CreateDefaultCarWithNewVincode("1_dealer_Actual_2"),
                CreateDefaultCarWithNewVincode("1_dealer_Actual_3"),
                CreateDefaultCarWithNewVincode("2_dealer_Actual_1"),
                CreateDefaultCarWithNewVincode("2_dealer_Actual_2"),
                CreateDefaultCarWithNewVincode("1_dealer_Sold_1"),
                CreateDefaultCarWithNewVincode("1_dealer_Sold_2"),
                CreateDefaultCarWithNewVincode("2_dealer_Sold_1"),
                CreateDefaultCarWithNewVincode("2_dealer_Sold_2"),
                CreateDefaultCarWithNewVincode("2_dealer_Sold_3")
            };
            foreach (var car in existedCars)
            {
                if (car.VinCode.Contains("_Actual_"))
                {
                    car.IsActual = true;
                    car.IsSold = false;
                }
                else if (car.VinCode.Contains("_Sold_"))
                {
                    car.IsActual = false;
                    car.IsSold = true;
                }

                if (car.VinCode.Contains("2_dealer"))
                    car.Dealer = secondDealer;
            }


            CreateDatabase();
            using (var context = new Context(_testContextOptions))
            {
                context.AddRange(existedCars);
                context.SaveChanges();
            }

            //Act
            List<Car> resultActuals;
            using (var context = new Context(_testContextOptions))
            {
                var sut = new CarRepository(context);
                resultActuals = sut.GetFullCarsInfoWithFilter(filter);
            }

            //Assert
            Assert.Equal(expectedCount, resultActuals.Count);
            foreach (var car in resultActuals)
                Assert.Equal(dealerName, car.Dealer.Name);
        }
        
        [Fact]
        public void Check_serialization_to_json()
        {
            //Arrange
            const string fileName = "test.json";
            var filter = new Filter
            {
                DealerName = DefaultDealer,
                Category = Category.Actual,
                FileFormat = FileFormat.json
            };
            
            var dcsMock = new Mock<IDataCheckerService>();
            var crMock = new Mock<ICarRepository>();

            crMock.Setup(cr => cr.GetFullCarsInfoWithFilter(filter))
                  .Returns(new List<Car> { _defaultDbCar });
            
            //Act
            var sut = new FileService(dcsMock.Object, crMock.Object);
            var file = sut.CreateFile(fileName, filter);

            //Assert
            var result = Assert.IsAssignableFrom<ActionResult>(file) as FileContentResult;
            Assert.NotNull(result);
            Assert.Equal(fileName, result.FileDownloadName);
            Assert.Equal( "application/octet-stream", result.ContentType);
            Assert.Equal("dafsdfa", result.FileContents.ToString());

        }
    }
}