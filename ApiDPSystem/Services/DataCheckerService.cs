using System.Collections.Generic;
using System.Linq;
using ApiDPSystem.Entities;
using ApiDPSystem.Repository;
using ApiDPSystem.Repository.Interfaces;
using ApiDPSystem.Services.Interfaces;

namespace ApiDPSystem.Services
{
    public class DataCheckerService : IDataCheckerService
    {
        private readonly ICarRepository _carRepository;

        public DataCheckerService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public void MarkSoldCars(List<Car> newCars, string dealerName)
        {
            var currentCarsVinCodes = _carRepository.GetActualCarsVinCodesForDealer(dealerName);
            var newCarsVinCodes = newCars.Select(p => p.VinCode).ToList();
            var soldCarVinCodes = currentCarsVinCodes.Except(newCarsVinCodes).ToList();

            _carRepository.MarkSoldCars(soldCarVinCodes);
        }

        public void SetToDatabase(List<Car> models)
        {
            //foreach (var model in models)
            for (var i = 0; i < models.Count; i++)
            {
                var model = models[i];

                var dealerName = model.Dealer.Name;

                SetColorIdIfExist(model);
                SetCarImageIdsIfExist(model);
                SetDealerIdIfExist(model);
                SetCarFeatureIdsIfExist(model);
                SetEngineIdIfExist(model);
                SetProducerIdIfExist(model);
                SetConfigurationIdIfExist(model);

                var existedCar = _carRepository.GetLastVersionCarWithVincodeAndDealerName(model.VinCode, dealerName);
                if (existedCar is not null)
                {
                    if (existedCar.IsSold)
                    {
                        model.Version = existedCar.Version + 1;
                    }
                    else if (IsCarModified(model, existedCar))
                    {
                        _carRepository.SetAsNotActual(existedCar);
                        model.Version = existedCar.Version + 1;
                    }
                }
                else
                {
                    model.Version = 1;
                }
            }
            _carRepository.AddCarRangeToDb(models);
        }

        private void SetColorIdIfExist(Car model)
        {
            var existedInteriorColor = _carRepository.GetColorIfExist(model.InteriorColor);
            if (existedInteriorColor != null)
            {
                model.InteriorColor = null;
                model.InteriorColorId = existedInteriorColor.Id;
            }

            var existedExteriorColor = _carRepository.GetColorIfExist(model.ExteriorColor);
            if (existedExteriorColor != null)
            {
                model.ExteriorColor = null;
                model.ExteriorColorId = existedExteriorColor.Id;
            }

            if (model.ExteriorColor != null && model.ExteriorColor.Equals(model.InteriorColor))
                model.ExteriorColor = model.InteriorColor;
        }

        private void SetCarImageIdsIfExist(Car model)
        {
            foreach (var carImage in model.CarImages)
            {
                var existedImage = _carRepository.GetImageIfExist(carImage.Image);
                if (existedImage != null)
                {
                    carImage.Image = null;
                    carImage.ImageId = existedImage.Id;
                }
            }
        }

        private void SetDealerIdIfExist(Car model)
        {
            var existedDealer = _carRepository.GetDealerIfExist(model.Dealer);
            if (existedDealer != null)
            {
                model.Dealer = null;
                model.DealerId = existedDealer.Id;
            }
        }

        private void SetConfigurationIdIfExist(Car model)
        {
            var existedConfiguration = _carRepository.GetConfigurationId(model.Configuration);

            if (existedConfiguration != null)
            {
                model.Configuration = null;
                model.ConfigurationId = existedConfiguration.Id;
            }
        }

        private void SetEngineIdIfExist(Car model)
        {
            var existedEngine = _carRepository.GetEngine(model.Configuration.Engine);

            if (existedEngine != null)
            {
                model.Configuration.Engine = null;
                model.Configuration.EngineId = existedEngine.Id;
            }
        }

        private void SetProducerIdIfExist(Car model)
        {
            var existProducer = _carRepository.GetProducer(model.Configuration.Producer);

            if (existProducer != null)
            {
                model.Configuration.Producer = null;
                model.Configuration.ProducerId = existProducer.Id;
            }
        }

        private void SetCarFeatureIdsIfExist(Car model)
        {
            foreach (var carFeature in model.CarFeatures)
            {
                var existedFeature = _carRepository.GetFeatureIfExist(carFeature.Feature);
                if (existedFeature != null)
                {
                    carFeature.Feature = null;
                    carFeature.FeatureId = existedFeature.Id;
                }
            }
        }

        private bool IsCarModified(Car newCar, Car existedCar)
        {
            if (newCar.ConfigurationId != existedCar.ConfigurationId) return true;
            if (newCar.ExteriorColorId != existedCar.ExteriorColorId) return true;
            if (newCar.InteriorColorId != existedCar.InteriorColorId) return true;
            if (newCar.DealerId != existedCar.DealerId) return true;
            if (newCar.Price != existedCar.Price) return true;

            var existedCarFeatures = _carRepository.GetCarFeaturesByCarId(existedCar.Id);
            if (newCar.CarFeatures.Count() != existedCarFeatures.Count()) return true;
            var existedCarFeaturesIds = existedCarFeatures.OrderBy(p => p.FeatureId).Select(p => p.FeatureId).ToList();
            var newCarFeaturesIds = newCar.CarFeatures.OrderBy(p => p.FeatureId).Select(p => p.FeatureId).ToList();
            if (!existedCarFeaturesIds.SequenceEqual(newCarFeaturesIds)) return true;

            var existedCarImages = _carRepository.GetCarImagesListByCarId(existedCar.Id);
            if (newCar.CarImages.Count() != existedCarImages.Count()) return true;
            var existedCarImagesIds = existedCarImages.OrderBy(p => p.ImageId).Select(p => p.ImageId).ToList();
            var newCarImagesIds = newCar.CarImages.OrderBy(p => p.ImageId).Select(p => p.ImageId).ToList();
            if (!existedCarImagesIds.SequenceEqual(newCarImagesIds)) return true;

            return false;
        }
    }
}