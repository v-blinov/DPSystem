using ApiDPSystem.Data;
using ApiDPSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiDPSystem.Repository
{
    public class CarRepository
    {
        private readonly Context _context;

        public CarRepository(Context context)
        {
            _context = context;
        }
        
        public List<string> GetActualCarsVinCodesForDealer(string dealerName) =>
            _context.Cars
            .Include(p => p.Dealer)
            .Where(p => p.Dealer.Name == dealerName && p.IsActual)
            .Select(p => p.VinCode)
            .ToList();

        public Car GetLastVersionCarWithVincodeAndDealerName(string vincode, string dealer)
        {
            var chosenCars = _context.Cars.Include(p => p.Dealer)
                                      .Where(p => p.Dealer.Name == dealer && p.VinCode == vincode)
                                      .ToList();
            return chosenCars.FirstOrDefault(p => p.Version == chosenCars.Max(s => s.Version));
        }

        public void MarkSoldCars(List<string> soldCarVincodes)
        {
            foreach (var vincode in soldCarVincodes)
            {
                var car = _context.Cars.FirstOrDefault(s => s.VinCode == vincode);
                if (car != null)
                {
                    car.IsSold = true;
                    car.IsActual = false;

                    _context.SaveChanges();
                }
            }
        }

        public Configuration GetConfigurationId(Configuration configuration)
        {
            if (configuration == null)
                return null;
            return _context.Configurations
                           .Include(p => p.Engine)
                           .Include(p => p.Producer)
                           .ToList()
                           .FirstOrDefault(p => configuration.Equals(p));
        }
        public Engine GetEngine(Engine engine)
        {
            if (engine == null)
                return null;
            return _context.Engines
                           .ToList()
                           .FirstOrDefault(p => engine.Equals(p));
        }
        public Producer GetProducer(Producer producer)
        {
            if (producer == null)
                return null;
            return _context.Producers
                           .ToList()
                           .FirstOrDefault(p => producer.Equals(p));
        }
        public Color GetColorIfExist(Color color)
        {
            if (color == null)
                return null;
            return _context.Colors
                           .ToList()
                           .FirstOrDefault(p => color.Equals(p));
        }
        public Image GetImageIfExist(Image image)
        {
            if (image == null)
                return null;

            return _context.Images
                           .ToList()
                           .FirstOrDefault(p => image.Equals(p));
        }
        public Dealer GetDealerIfExist(Dealer dealer)
        {
            if (dealer == null)
                return null;
            return _context.Dealers
                           .ToList()
                           .FirstOrDefault(p => dealer.Equals(p));
        }
        public Feature GetFeatureIfExist(Feature feature)
        {
            if (feature == null)
                return null;
            return _context.Features
                           .ToList()
                           .FirstOrDefault(p => feature.Equals(p));
        }
        
        public void AddCarToDb(Car model)
        {
            _context.Cars.Add(model);
            _context.SaveChanges();
        }

        public void AddCarRangeToDb(List<Car> models)
        {
            _context.Cars.AddRange(models);
            _context.SaveChanges();
        }

        public List<CarImage> GetCarImagesListByCarId(Guid carId) =>
            _context.CarImages.Where(p => p.CarId == carId).ToList();

        public List<CarFeature> GetCarFeaturesByCarId(Guid carId) =>
            _context.CarFeatures.Where(p => p.CarId == carId).ToList();
        public void SetAsNotActual(Car existedCar)
        {
            var car = _context.Cars.FirstOrDefault(p => p.Id == existedCar.Id);
            if (car != null) car.IsActual = false;
            _context.SaveChanges();
        }

    }
}