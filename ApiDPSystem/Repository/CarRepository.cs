﻿using ApiDPSystem.Data;
using ApiDPSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using ApiDPSystem.Repository.Interfaces;

namespace ApiDPSystem.Repository
{
    public class CarRepository : ICarRepository
    {
        private readonly Context _context;

        public CarRepository(Context context)
        {
            _context = context;
        }

        public Configuration ReturnConfigurationIfExist(Configuration configuration)
        {
            if (configuration == null)
                return null;

            return _context.Configurations
                           .Include(p => p.Engine)
                           .Include(p => p.Producer)
                           .ToList()
                           .FirstOrDefault(p => configuration.Equals(p));
        }

        public Producer ReturnProducerIfExist(Producer producer)
        {
            if (producer == null)
                return null;

            return _context.Producers
                           .ToList()
                           .FirstOrDefault(p => producer.Equals(p));
        }

        public Engine ReturnEngineIfExist(Engine engine)
        {
            if (engine == null)
                return null;

            return _context.Engines
                           .ToList()
                           .FirstOrDefault(p => engine.Equals(p));
        }

        public Feature ReturnFeatureIfExist(Feature feature)
        {
            if (feature == null)
                return null;

            return _context.Features
                           .ToList()
                           .FirstOrDefault(p => feature.Equals(p));
        }

        public Color ReturnColorIfExist(Color color)
        {
            if (color == null)
                return null;

            return _context.Colors
                           .ToList()
                           .FirstOrDefault(p => color.Equals(p));
        }

        public Image ReturnImageIfExist(Image image)
        {
            if (image == null)
                return null;

            return _context.Images
                           .ToList()
                           .FirstOrDefault(p => image.Equals(p));
        }

        public Dealer ReturnDealerIfExist(Dealer dealer)
        {
            if (dealer == null)
                return null;

            return _context.Dealers
                           .ToList()
                           .FirstOrDefault(p => dealer.Equals(p));
        }



        public CarActual GetThatDealerCarIfExist(CarActual model) =>
            _context.CarActuals
            .Include(p => p.CarImages)
            .FirstOrDefault(p => p.VinCode == model.VinCode && p.DealerId == model.DealerId);

        public List<string> GetActualCarsVinCodesForDealer(string dealerName) =>
            _context.CarActuals
            .Include(p => p.Dealer)
            .Where(p => p.Dealer.Name == dealerName)
            .Select(p => p.VinCode)
            .ToList();

        public CarActual GetCar(string vincode, string dealerName) =>
            _context.CarActuals
            .Include(p => p.Dealer)
            .Include(p => p.CarImages)
            .FirstOrDefault(p => p.Dealer.Name == dealerName && p.VinCode == vincode);


        public void TransferOneCarFromActualToHistory(CarActual car, bool isSold = false)
        {
            var transaction = _context.Database.BeginTransaction();

            try
            {
                var carHistoryModel = new CarHistory();
                carHistoryModel.Copy(car);

                foreach (var carImage in car.CarImages)
                {
                    carHistoryModel.CarHistoryImages.Add(new CarHistoryImage { ImageId = carImage.ImageId });
                    _context.CarImages.RemoveRange(carImage);
                }

                _context.CarActuals.Remove(car);
                carHistoryModel.IsSold = isSold;
                _context.CarHistories.Add(carHistoryModel);
                _context.SaveChanges();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Log.Error("Ошибка при переносе данных из таблицы CarEntities в таблицу SoldCars");
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public void AddCarToDb(CarActual model)
        {
            var maxVersion = GetMaxVersionByVincode(model.VinCode);
            model.Version = maxVersion != null ? (int)maxVersion + 1 : 1;
            _context.CarActuals.Add(model);
            _context.SaveChanges();
        }

        private int? GetMaxVersionByVincode(string vinCode)
        {
            var actualVersions = _context.CarActuals.Where(p => p.VinCode == vinCode).ToList();
            var historyVersions = _context.CarHistories.Where(p => p.VinCode == vinCode).ToList();

            int? maxActualVersion = actualVersions.Count > 0 ? actualVersions.Max(p => p.Version) : null;
            int? maxHistoryVersion = historyVersions.Count > 0 ? historyVersions.Max(p => p.Version) : null;

            if (maxActualVersion != null)
                if (maxHistoryVersion != null)
                    return Math.Max((int)maxActualVersion, (int)maxHistoryVersion);
                else
                    return maxActualVersion;

            return maxHistoryVersion;
        }

        public List<CarImage> GetCarImagesListByCarId(Guid carId) =>
            _context.CarImages.Where(p => p.CarActualId == carId).ToList();
    }
}