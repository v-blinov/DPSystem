using ApiDPSystem.Data;
using ApiDPSystem.Entities;
using ApiDPSystem.Repository;
using ApiDPSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DPSystem.Tests
{
    public class FileConverterTests
    {
        private const string DefaultDealer = "DefaultDealer";
        private readonly CarActual _defaultExistedCar = new()
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
                Drive = "AWD",
                ConfigurationFeatures = new List<ConfigurationFeature>
                    {
                        new ConfigurationFeature
                            {Feature = new Feature {Type = "Safety", Description = "Система контроля слепых зон"}}
                    }
            },
            ExteriorColor = new Color { Name = "Marina blue" },
            InteriorColor = new Color { Name = "White" },
            CarImages = new List<CarImage> { new CarImage { Image = new Image { Url = "/test/url.png" } } },
            Price = 1000000
        };

        private const string TestConnectionString = "Server=mssql,8083;Database=DPSystem.Tests;User=sa;Password=Qwerty123!;";
        private readonly DbContextOptions<Context> _testContextOptions = new DbContextOptionsBuilder<Context>().UseSqlServer(TestConnectionString).Options;

        private void CreateDatabase()
        {
            using var context = new Context(_testContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        private CarActual CreateDefaultCarWithVincode(string vincode)
        {
            var testCar = new CarActual();
            testCar.Copy(_defaultExistedCar);
            testCar.VinCode = vincode;
            testCar.Version = 1;
            return testCar;
        }


        [Fact]
        public void Dealer_has_no_cars_in_db_positive_test()
        {
            //Arrange
            List<CarActual> newCars = new()
            {
                CreateDefaultCarWithVincode("new_Vincode1"),
                CreateDefaultCarWithVincode("new_Vincode2"),
                CreateDefaultCarWithVincode("new_Vincode3")
            };
            CreateDatabase();

            //Act
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                var sut = new DataCheckerService(carRepository);

                sut.TransferSoldCars(newCars, DefaultDealer);
            }

            //Assert
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                
                Assert.Equal(0, context.CarHistories.Count());
                Assert.Equal(0, context.CarActuals.Count());
            }
        }

        [Fact]
        public void Dealer_has_cars_in_db_without_matches_with_new_cars_positive_test()
        {
            //Arrange
            List<CarActual> existedCars = new()
            {
                CreateDefaultCarWithVincode("existed_Vincode1"),
                CreateDefaultCarWithVincode("existed_Vincode2")
            };
            List<CarActual> newCars = new()
            {
                CreateDefaultCarWithVincode("new_Vincode1"),
                CreateDefaultCarWithVincode("new_Vincode2"),
                CreateDefaultCarWithVincode("new_Vincode3")
            };

            CreateDatabase();
            using (var context = new Context(_testContextOptions))
            {
                context.CarActuals.AddRange(existedCars);
                context.SaveChanges();
            }

            //Act
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                var sut = new DataCheckerService(carRepository);

                sut.TransferSoldCars(newCars, DefaultDealer);
            }

            //Assert
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);

                Assert.Equal(2, context.CarHistories.Count());
                Assert.Equal(0, context.CarActuals.Count());
            }
        }

        [Fact]
        public void Dealer_has_cars_in_db_with_the_same_vin_as_in_newCarsList_positive_test()
        {
            //Arrange
            List<CarActual> existedCars = new()
            {
                CreateDefaultCarWithVincode("existed_Vincode1"),
                CreateDefaultCarWithVincode("Vincode2"),
                CreateDefaultCarWithVincode("Vincode3")
            };
            List<CarActual> newCars = new()
            {
                CreateDefaultCarWithVincode("new_Vincode1"),
                CreateDefaultCarWithVincode("Vincode2"),
                CreateDefaultCarWithVincode("Vincode3")
            };

            CreateDatabase();
            using (var context = new Context(_testContextOptions))
            {
                context.CarActuals.AddRange(existedCars);
                context.SaveChanges();
            }

            //Act
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                var sut = new DataCheckerService(carRepository);

                sut.TransferSoldCars(newCars, DefaultDealer);
            }

            //Assert
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);

                Assert.Equal(1, context.CarHistories.Count());
                Assert.Equal(2, context.CarActuals.Count());
            }
        }

        [Fact]
        public void NewCarsList_is_empty_positive_test()
        {
            //Assert
            List<CarActual> existedCars = new()
            {
                CreateDefaultCarWithVincode("existed_Vincode1"),
                CreateDefaultCarWithVincode("existed_Vincode2")
            };
            List<CarActual> newCars = new() { };

            CreateDatabase();
            using (var context = new Context(_testContextOptions))
            {
                context.CarActuals.AddRange(existedCars);
                context.SaveChanges();
            }

            //Act
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                var sut = new DataCheckerService(carRepository);

                sut.TransferSoldCars(newCars, DefaultDealer);
            }

            //Assert
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);

                Assert.Equal(2, context.CarHistories.Count());
                Assert.Equal(0, context.CarActuals.Count());
            }
        }


        [Fact]
        public void There_are_cars_in_db_from_another_dealer()
        {
            var anotherDealerCar1 = CreateDefaultCarWithVincode("existed_another1");
            var anotherDealerCar2 = CreateDefaultCarWithVincode("existed_another2");

            anotherDealerCar1.Dealer = new Dealer {Name = "AnotherDealer"};
            anotherDealerCar2.Dealer = new Dealer {Name = "AnotherDealer"};

            //Arrange
            List<CarActual> existedCars = new()
            {
                CreateDefaultCarWithVincode("Vincode1"),
                CreateDefaultCarWithVincode("existed_Vincode2"),
                anotherDealerCar1,
                anotherDealerCar2

            };
            List<CarActual> newCars = new()
            {
                CreateDefaultCarWithVincode("Vincode1"),
                CreateDefaultCarWithVincode("new_Vincode2"),
                CreateDefaultCarWithVincode("new_Vincode3")
            };

            CreateDatabase();
            using (var context = new Context(_testContextOptions))
            {
                context.CarActuals.AddRange(existedCars);
                context.SaveChanges();
            }

            //Act
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                var sut = new DataCheckerService(carRepository);

                sut.TransferSoldCars(newCars, DefaultDealer);
            }

            //Assert
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);

                Assert.Equal(1, context.CarHistories.Count());
                Assert.Equal(3, context.CarActuals.Count());
            }
        }
    }
}