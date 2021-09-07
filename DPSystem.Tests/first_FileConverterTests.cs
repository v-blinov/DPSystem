using ApiDPSystem.Entities;
using ApiDPSystem.Repository.Interfaces;
using ApiDPSystem.Services;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace DPSystem.Tests
{
    public class FileConverterTests
    {
        private const string DefaultDealer = "DefaultDealer";

        [Fact]
        public void Dealer_has_no_cars_in_db_positive_test()
        {
            //Arrange
            var newCars = new List<CarActual>
            {
                new() { VinCode = "111" },
                new() { VinCode = "222" },
                new() { VinCode = "333" }
            };

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock
                .Setup(repo => repo.GetActualCarsVinCodesForDealer(DefaultDealer))
                .Returns(new List<string>());

            var sut = new DataCheckerService(carRepositoryMock.Object);

            //Act
            sut.TransferSoldCars(newCars, DefaultDealer);

            //Assert
            carRepositoryMock.Verify(p => p.GetCar(null, DefaultDealer), Times.Never);
            carRepositoryMock.Verify(p => p.TransferOneCarFromActualToHistory(new CarActual(), true), Times.Never);
        }

        [Fact]
        public void Dealer_has_cars_in_db_without_matches_with_new_cars_positive_test()
        {
            //Arrange
            var newCars = new List<CarActual>
            {
                new() { VinCode = "newVin1" },
                new() { VinCode = "newVin2" },
                new() { VinCode = "newVin3" }
            };
            var currentDealerCarsVins = new List<string>
            {
                "oldVin4",
                "oldVin5"
            };

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock
                .Setup(repo => repo.GetActualCarsVinCodesForDealer(DefaultDealer))
                .Returns(currentDealerCarsVins);

            var sut = new DataCheckerService(carRepositoryMock.Object);

            //Act
            sut.TransferSoldCars(newCars, DefaultDealer);

            //Assert
            carRepositoryMock.Verify(p => p.GetCar(null, DefaultDealer), Times.Never);
            carRepositoryMock.Verify(p => p.TransferOneCarFromActualToHistory(new CarActual(), true), Times.Never);
        }

        [Fact]
        public void Dealer_has_cars_in_db_with_the_same_vin_as_in_newCarsList_positive_test()
        {
            //Arrange
            var newCars = new List<CarActual>
            {
                new() { VinCode = "newVin1" },
                new() { VinCode = "newVin2" },
                new() { VinCode = "newVin3" }
            };
            var currentDealerCarsVins = new List<string>
            {
                "newVin2",
                "newVin3",
                "oldVin4",
                "oldVin5"
            };

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock
                .Setup(repo => repo.GetActualCarsVinCodesForDealer(DefaultDealer))
                .Returns(currentDealerCarsVins);
            carRepositoryMock
                .Setup(p => p.GetCar("vincode", "dealerName"))
                .Returns(new CarActual() {VinCode = "soldCar"});

            var sut = new DataCheckerService(carRepositoryMock.Object);

            //Act
            sut.TransferSoldCars(newCars, DefaultDealer);

            //Assert
            carRepositoryMock.Verify(p => p.GetCar(null, DefaultDealer), Times.Never);
            carRepositoryMock.Verify(p => p.TransferOneCarFromActualToHistory(null, true), Times.Exactly(2));
        }

        [Fact]
        public void newCarsList_is_empty_positive_test()
        {
            //Arrange
            var newCars = new List<CarActual>();
            var currentDealerCarsVins = new List<string>
            {
                "oldVin1",
                "oldVin2"
            };

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock
                .Setup(repo => repo.GetActualCarsVinCodesForDealer(DefaultDealer))
                .Returns(currentDealerCarsVins);
            carRepositoryMock
                .Setup(p => p.GetCar("vincode", "dealerName"))
                .Returns(new CarActual() { VinCode = "soldCar" });

            var sut = new DataCheckerService(carRepositoryMock.Object);

            //Act
            sut.TransferSoldCars(newCars, DefaultDealer);

            //Assert
            carRepositoryMock.Verify(p => p.GetCar(null, DefaultDealer), Times.Never);
            carRepositoryMock.Verify(p => p.TransferOneCarFromActualToHistory(null, true), Times.Exactly(2));
        }
    }
}