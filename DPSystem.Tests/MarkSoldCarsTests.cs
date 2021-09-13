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
    public class FileDataOperationsTests
    {
        private const string DefaultDealer = "DefaultDealer";
        private readonly Car _defaultExistedCar = new()
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
            },
            CarFeatures = new List<CarFeature>
            {
                new () { Feature = new Feature { Type = "Safety", Description = "Система контроля слепых зон" } }
            },
            ExteriorColor = new Color { Name = "Marina blue" },
            InteriorColor = new Color { Name = "White" },
            CarImages = new List<CarImage> { new CarImage { Image = new Image { Url = "/test/url.png" } } },
            Price = 1000000,
            Version = 1
        };

        private const string TestConnectionString = "Server=mssql,8083;Database=DPSystem.Tests;User=sa;Password=Qwerty123!;";
        private readonly DbContextOptions<Context> _testContextOptions = new DbContextOptionsBuilder<Context>().UseSqlServer(TestConnectionString).Options;

        private void CreateDatabase()
        {
            using var context = new Context(_testContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        private void SetDefaultCarToDatabase()
        {
            using var context = new Context(_testContextOptions);
            context.Cars.Add(_defaultExistedCar);
            context.SaveChanges();
        }
        private Car CreateDefaultCarNewWithVincode(string vincode)
        {
            var copy = _defaultExistedCar.Copy();
            copy.VinCode = vincode;
            return copy;
        }
        

        [Fact]
        public void Add_cars_to_db__params_3thesame_0new__expected_0sold_3actual()
        {
            //Arrange
            var oldCars = new List<Car>()
            {
                CreateDefaultCarNewWithVincode("Vincode1"),
                CreateDefaultCarNewWithVincode("Vincode2"),
                CreateDefaultCarNewWithVincode("Vincode3"),
            };
            var newCars = new List<Car>()
            {
                CreateDefaultCarNewWithVincode("Vincode1"),
                CreateDefaultCarNewWithVincode("Vincode2"),
                CreateDefaultCarNewWithVincode("Vincode3"),
            };
            
            CreateDatabase();
            using (var context = new Context(_testContextOptions))
            {
                context.Cars.AddRange(oldCars);
                context.SaveChanges();
            }
            
            //Act
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                var sut = new DataCheckerService(carRepository);
            
                sut.MarkSoldCars(newCars, DefaultDealer);
                sut.SetToDatabase(newCars);
            }
            
            //Assert
            using (var context = new Context(_testContextOptions))
            {
                var actualCars = context.Cars.Where(p => p.IsActual).Select(p => new {p.VinCode, p.Version, p.IsActual, p.IsSold}).ToList();
                var soldCars = context.Cars.Where(p => p.IsSold).Select(p => new {p.VinCode, p.Version, p.IsActual, p.IsSold}).ToList();
               
                Assert.Equal(3, actualCars.Count());                
                Assert.Equal(0, soldCars.Count());
                Assert.Empty(actualCars.Intersect(soldCars));
            }
        }

        [Fact]
        public void Add_cars_to_db__params_1thesame_2new__expected_2sold_3actual()
        {
            //Arrange
            var oldCars = new List<Car>()
            {
                CreateDefaultCarNewWithVincode("old_Vincode1"),
                CreateDefaultCarNewWithVincode("old_Vincode2"),
                CreateDefaultCarNewWithVincode("old_Vincode3"),
            };
            var newCars = new List<Car>()
            {
                CreateDefaultCarNewWithVincode("new_Vincode1"),
                CreateDefaultCarNewWithVincode("new_Vincode2"),
                CreateDefaultCarNewWithVincode("old_Vincode2"),
            };
            
            CreateDatabase();
            using (var context = new Context(_testContextOptions))
            {
                context.Cars.AddRange(oldCars);
                context.SaveChanges();
            }
            
            
            //Act
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                var sut = new DataCheckerService(carRepository);
            
                sut.MarkSoldCars(newCars, DefaultDealer);
                sut.SetToDatabase(newCars);
            }
            
            //Assert
            //using (var context = new Context(_testContextOptions))
            //{
            //    var old2 = context.Cars.FirstOrDefault(p => p.VinCode == "old_Vincode2");
            //    var actualCars = context.Cars.Where(p => p.IsActual).Select(p => new {p.VinCode, p.Version, p.IsActual, p.IsSold}).ToList();
            //    var soldCars = context.Cars.Where(p => p.IsSold).Select(p => new {p.VinCode, p.Version, p.IsActual, p.IsSold}).ToList();
               
            //    //Assert.False(old2?.IsSold);
            //    Assert.Equal(3, actualCars.Count());                
            //    Assert.Equal(2, soldCars.Count());
            //    Assert.Empty(actualCars.Intersect(soldCars));
            //}
        }
        
        [Fact]
        public void Add_cars_to_db__params_0thesame_3new__expected_3sold_0actual()
        {
            //Arrange
            var oldCars = new List<Car>()
            {
                CreateDefaultCarNewWithVincode("old_Vincode1"),
                CreateDefaultCarNewWithVincode("old_Vincode2"),
                CreateDefaultCarNewWithVincode("old_Vincode3"),
            };
            var newCars = new List<Car>()
            {
                CreateDefaultCarNewWithVincode("new_Vincode1"),
                CreateDefaultCarNewWithVincode("new_Vincode2"),
                CreateDefaultCarNewWithVincode("new_Vincode3"),
            };

            CreateDatabase();
            using (var context = new Context(_testContextOptions))
            {
                context.Cars.AddRange(oldCars);
                context.SaveChanges();
            }

            //Act
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                var sut = new DataCheckerService(carRepository);

                sut.MarkSoldCars(newCars, DefaultDealer);
            }

            //Assert
            using (var context = new Context(_testContextOptions))
            {
                //var actualCars = context.Cars.Where(p => p.IsActual).Select(p => new { p.VinCode, p.Version, p.IsActual, p.IsSold }).ToList();
                var soldCars = context.Cars.Where(p => p.IsSold).Select(p => new { p.VinCode, p.Version, p.IsActual, p.IsSold }).ToList();

                //Assert.Equal(3, actualCars.Count());
                Assert.Equal(3, soldCars.Count());
                //Assert.Empty(actualCars.Intersect(soldCars));
            }
        }

        [Fact]
        public void Add_cars_to_db__params_1sold_1thesame_1new__expected_3sold_3actual()
        {
            //Arrange
            var soldCar = CreateDefaultCarNewWithVincode("old_Vincode");
            soldCar.Version = 1;

            var oldCars = new List<Car>()
            {
                soldCar,
                CreateDefaultCarNewWithVincode("old_Vincode1"),
                CreateDefaultCarNewWithVincode("old_Vincode2"),
                CreateDefaultCarNewWithVincode("old_Vincode3"),
            };
            var newCars = new List<Car>()
            {
                CreateDefaultCarNewWithVincode("old_Vincode"),
                CreateDefaultCarNewWithVincode("old_Vincode1"),
                CreateDefaultCarNewWithVincode("new_Vincode3"),
            };

            CreateDatabase();
            using (var context = new Context(_testContextOptions))
            {
                context.Cars.AddRange(oldCars);
                context.SaveChanges();

                var car = context.Cars.FirstOrDefault(p => p.VinCode == "old_Vincode");
                car.IsActual = false;
                car.IsSold = true;
                context.SaveChanges();
            }

            //Act
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                var sut = new DataCheckerService(carRepository);

                sut.MarkSoldCars(newCars, DefaultDealer);
                sut.SetToDatabase(newCars);
            }

            //Assert
            using (var context = new Context(_testContextOptions))
            {
                var actualCars = context.Cars.Where(p => p.IsActual).Select(p => new { p.VinCode, p.Version, p.IsActual, p.IsSold }).ToList();
                var soldCars = context.Cars.Where(p => p.IsSold).Select(p => new { p.VinCode, p.Version, p.IsActual, p.IsSold }).ToList();

                Assert.Equal(3, actualCars.Count());
                Assert.Equal(3, soldCars.Count());
                Assert.Empty(actualCars.Intersect(soldCars));
                Assert.Equal(2, context.Cars.Where(p => p.VinCode == "old_Vincode").ToList().Count());
                Assert.Equal(2, context.Cars.FirstOrDefault(p => p.VinCode == "old_Vincode" && p.IsActual && !p.IsSold)?.Version);
            }
        }


        [Fact]
        public void Set_car_with_not_existed_engine()
        {
            //Arrange
            CreateDatabase();
            SetDefaultCarToDatabase();

            var carWithNewEngine = CreateDefaultCarNewWithVincode("new_Vincode1");
            carWithNewEngine.Configuration.Engine = new Engine { Fuel = "Electricity", Capacity = null, Power = 600 };

            //Act
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                var sut = new DataCheckerService(carRepository);

                sut.SetToDatabase(new List<Car> { carWithNewEngine });
            }

            //Assert
            using (var context = new Context(_testContextOptions))
            {
                Assert.Equal(2, context.Cars.Count());
                Assert.Equal(2, context.Colors.Count());
                Assert.Equal(2, context.Configurations.Count());
                Assert.Equal(1, context.Features.Count());
                Assert.Equal(1, context.Dealers.Count());
                Assert.Equal(2, context.Engines.Count());
                Assert.Equal(1, context.Images.Count());
                Assert.Equal(1, context.Producers.Count());
            }
        }

        [Fact]
        public void Set_car_with_not_existed_producer()
        {
            //Arrange
            CreateDatabase();
            SetDefaultCarToDatabase();

            var carWithNewEngine = CreateDefaultCarNewWithVincode("new_Vincode1");
            carWithNewEngine.Configuration.Producer = new Producer() {Name = "NewProducer"};

            //Act
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                var sut = new DataCheckerService(carRepository);

                sut.SetToDatabase(new List<Car> { carWithNewEngine });
            }

            //Assert
            using (var context = new Context(_testContextOptions))
            {
                Assert.Equal(2, context.Cars.Count());
                Assert.Equal(2, context.Colors.Count());
                Assert.Equal(2, context.Configurations.Count());
                Assert.Equal(1, context.Features.Count());
                Assert.Equal(1, context.Dealers.Count());
                Assert.Equal(1, context.Engines.Count());
                Assert.Equal(1, context.Images.Count());
                Assert.Equal(2, context.Producers.Count());
            }
        }

        [Fact]
        public void Set_car_which_has_interior_and_exterior_color_the_same()
        {
            //Arrange
            CreateDatabase();
            SetDefaultCarToDatabase();

            var theSameCar = CreateDefaultCarNewWithVincode("new_Vincode1");
            theSameCar.InteriorColor = new Color() { Name = "Brown" };
            theSameCar.ExteriorColor = new Color() { Name = "Brown" };

            //Act
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                var sut = new DataCheckerService(carRepository);

                sut.SetToDatabase(new List<Car> { theSameCar });
            }

            //Assert
            using (var context = new Context(_testContextOptions))
            {
                Assert.Equal(2, context.Cars.Count());
                Assert.Equal(3, context.Colors.Count());
                Assert.Equal(2, context.Configurations.Count());
                Assert.Equal(1, context.Features.Count());
                Assert.Equal(1, context.Dealers.Count());
                Assert.Equal(1, context.Engines.Count());
                Assert.Equal(1, context.Images.Count());
                Assert.Equal(1, context.Producers.Count());
            }
        }

        [Fact]
        public void Set_car_with_new_features()
        {
            //Arrange
            CreateDatabase();
            SetDefaultCarToDatabase();

            var theSameCar = CreateDefaultCarNewWithVincode("new_Vincode1");
            theSameCar.CarFeatures = new List<CarFeature>()
            {
                new CarFeature()
                    {Feature = new Feature() {Type = "Safety", Description = "New Safety feature"}},
                new CarFeature()
                    {Feature = new Feature() {Type = "Interior", Description = "New Interior feature"}}
            };

            //Act
            using (var context = new Context(_testContextOptions))
            {
                var carRepository = new CarRepository(context);
                var sut = new DataCheckerService(carRepository);

                sut.SetToDatabase(new List<Car> { theSameCar });
            }

            //Assert
            using (var context = new Context(_testContextOptions))
            {
                Assert.Equal(2, context.Cars.Count());
                Assert.Equal(2, context.Colors.Count());
                Assert.Equal(1, context.Configurations.Count());
                Assert.Equal(3, context.Features.Count());
                Assert.Equal(1, context.Dealers.Count());
                Assert.Equal(1, context.Engines.Count());
                Assert.Equal(1, context.Images.Count());
                Assert.Equal(1, context.Producers.Count());
            }
        }
    }
}
