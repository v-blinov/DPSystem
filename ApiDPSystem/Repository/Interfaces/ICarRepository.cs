using System;
using System.Collections.Generic;
using ApiDPSystem.Entities;
using ApiDPSystem.Models;

namespace ApiDPSystem.Repository.Interfaces
{
    public interface ICarRepository
    {

        public List<string> GetActualCarsVinCodesForDealer(string dealerName);

        public Car GetLastVersionCarWithVincode(string vincode);

        public void MarkSoldCars(List<string> soldCarVincodes);

        public Configuration GetConfigurationId(Configuration configuration);
        public Engine GetEngine(Engine engine);
        public Producer GetProducer(Producer producer);
        public Color GetColorIfExist(Color color);
        public Image GetImageIfExist(Image image);
        public Dealer GetDealerIfExist(Dealer dealer);
        public Feature GetFeatureIfExist(Feature feature);

        public void AddCarToDb(Car model);

        public void AddCarRangeToDb(List<Car> models);

        public List<CarImage> GetCarImagesListByCarId(Guid carId);

        public List<CarFeature> GetCarFeaturesByCarId(Guid carId);
        public void SetAsNotActual(Car existedCar);
        public List<Car> GetFullCarsInfoWithFilter(Filter filter);
    }
}