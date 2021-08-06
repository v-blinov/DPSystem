using ApiDPSystem.Data;
using ApiDPSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDPSystem.Repository
{
    public class MapperRepository
    {
        private readonly Context _context;

        public MapperRepository(Context context)
        {
            _context = context;
        }

        public Configuration ReturnConfigurationIfExist(Configuration configuration)
        {
            if (configuration == null)
                return configuration;

            return _context.Configurations
                           .Include(p => p.Engine)
                           .Include(p => p.Producer)
                           .ToList()
                           .FirstOrDefault(p => configuration.Equals(p));
        }
        public Producer ReturnProducerIfExist(Producer producer)
        {
            if (producer == null)
                return producer;

            return _context.Producers
                           .ToList()
                           .FirstOrDefault(p => producer.Equals(p));
        }
        public Engine ReturnEngineIfExist(Engine engine)
        {
            if (engine == null)
                return engine;

            return _context.Engines
                           .ToList()
                           .FirstOrDefault(p => engine.Equals(p));
        }
        public Feature ReturnFeatureIfExist(Feature feature)
        {
            if (feature == null)
                return feature;

            return _context.Features
                           .ToList()
                           .FirstOrDefault(p => feature.Equals(p));
        }
        public Color ReturnColorIfExist(Color color)
        {
            if (color == null)
                return color;

            return _context.Colors
                           .ToList()
                           .FirstOrDefault(p => color.Equals(p));
        }
        public Image ReturnImageIfExist(Image image)
        { 
            if (image == null)
                return image;

            return _context.Images
                           .ToList()
                           .FirstOrDefault(p => image.Equals(p));
        }
        public Dealer ReturnDealerIfExist(Dealer dealer)
        {
            if (dealer == null)
                return dealer;

            return _context.Dealers
                           .ToList()
                           .FirstOrDefault(p => dealer.Equals(p));
        }

        public async Task AddCarEntityOrUpdateIfExist(CarEntity model)
        {
            var existedCar = _context.CarEntities
                .Include(p => p.CarImages)
                .FirstOrDefault(p => p.VinCode == model.VinCode && (p.Dealer == model.Dealer || p.DealerId == model.DealerId));

            if (existedCar == null)
            {
                _context.CarEntities.Add(model);
            }
            else
            {
                existedCar.Copy(model);
            }

            await _context.SaveChangesAsync();
        }

        public void TransferSoldCars(List<CarEntity> newListCars, string dealerName)
        {
            //распараллелить
            var currentListCarVinCodes = _context.CarEntities
                                        .Include(p => p.Dealer)
                                        .Where(p => p.Dealer.Name == dealerName)
                                        .Select(p => p.VinCode)
                                        .ToList();

            var newListCarVinCodes = newListCars
                                        .Select(p => p.VinCode)
                                        .ToList();

            var soldCarVinCodes = currentListCarVinCodes
                                        .Except(newListCarVinCodes)
                                        .ToList();

            var transaction = _context.Database.BeginTransaction();
            try
            {
                foreach (var vincode in soldCarVinCodes)
                {
                    var car = _context.CarEntities
                                        .Include(p => p.Dealer)
                                        .Include(p => p.CarImages)
                                        .FirstOrDefault(p => p.Dealer.Name == dealerName && p.VinCode == vincode);

                    var soldCar = new SoldCar();
                    soldCar.Copy(car);

                    foreach (var carImage in car.CarImages)
                    { 
                        soldCar.SoldCarImages.Add(new SoldCarImage { ImageId = carImage.ImageId });
                        _context.CarImages.RemoveRange(carImage);
                    }

                    _context.CarEntities.Remove(car);
                    _context.SoldCars.Add(soldCar);
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Log.Error("Ошибка при переносе данных из таблицы CarEntities в таблицу SoldCars");
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
    }
}
