using ApiDPSystem.Entities;
using ApiDPSystem.Repository;
using ApiDPSystem.Services.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ApiDPSystem.Services
{
    public class DataCheckerService : IDataCheckerService
    {
        private readonly CarRepository _carRepository;

        public DataCheckerService(CarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public void TransferSoldCars(List<CarActual> newListCars, string dealerName)
        {
            var currentCarsVinCodes = _carRepository.GetActualCarsVinCodesForDealer(dealerName);

            var newCarsVinCodes = newListCars.Select(p => p.VinCode).ToList();

            var soldCarVinCodes = currentCarsVinCodes
                                  .Except(newCarsVinCodes)
                                  .ToList();

            foreach (var vincode in soldCarVinCodes)
            {
                var car = _carRepository.GetCar(vincode, dealerName);
                _carRepository.TransferOneCarFromActualToHistory(car, true);
            }
        }

        public void SetToDatabase(List<CarActual> models)
        {
            foreach (var model in models)
            {
                SetConfigurationIdIfExist(model);
                
                // If configuration exists in DB
                if (model.Configuration != null)
                    SetConfigurationFeatureIdsIfExist(model);

                SetColorIdIfExist(model);
                SetCarImageIdsIfExist(model);
                SetDealerIdIfExist(model);

                // if dealer exists in DB
                if (model.Dealer == null)
                {
                    var existedCar = _carRepository.GetThatDealerCarIfExist(model);
                    if (existedCar != null)
                    {
                        if (!IsCarModified(model, existedCar))
                            continue;
                        else
                            _carRepository.TransferOneCarFromActualToHistory(existedCar);
                    }
                }

                _carRepository.AddCarToDb(model);
            }
        }

        private void SetConfigurationIdIfExist(CarActual model)
        {
            var exitedConfiguration = _carRepository.ReturnConfigurationIfExist(model.Configuration);
            if (exitedConfiguration != null)
            {
                model.Configuration = null;
                model.ConfigurationId = exitedConfiguration.Id;
                return;
            }

            var existedProducer = _carRepository.ReturnProducerIfExist(model.Configuration.Producer);
            if (existedProducer != null)
            {
                model.Configuration.Producer = null;
                model.Configuration.ProducerId = existedProducer.Id;
            }

            var existedEngine = _carRepository.ReturnEngineIfExist(model.Configuration.Engine);
            if (existedEngine != null)
            {
                model.Configuration.Engine = null;
                model.Configuration.EngineId = existedEngine.Id;
            }
        }

        private void SetConfigurationFeatureIdsIfExist(CarActual model)
        {
            foreach (var configurationFeature in model.Configuration.ConfigurationFeatures)
            {
                var existedFeature = _carRepository.ReturnFeatureIfExist(configurationFeature.Feature);
                if (existedFeature != null)
                {
                    configurationFeature.Feature = null;
                    configurationFeature.FeatureId = existedFeature.Id;
                }
            }
        }

        private void SetColorIdIfExist(CarActual model)
        {
            var existedInteriorColor = _carRepository.ReturnColorIfExist(model.InteriorColor);
            if (existedInteriorColor != null)
            {
                model.InteriorColor = null;
                model.InteriorColorId = existedInteriorColor.Id;
            }

            var existedExteriorColor = _carRepository.ReturnColorIfExist(model.ExteriorColor);
            if (existedExteriorColor != null)
            {
                model.ExteriorColor = null;
                model.ExteriorColorId = existedExteriorColor.Id;
            }

            if (model.ExteriorColor != null && model.ExteriorColor.Equals(model.InteriorColor))
                model.ExteriorColor = model.InteriorColor;
        }

        private void SetCarImageIdsIfExist(CarActual model)
        {
            foreach (var carImage in model.CarImages)
            {
                var existedImage = _carRepository.ReturnImageIfExist(carImage.Image);
                if (existedImage != null)
                {
                    carImage.Image = null;
                    carImage.ImageId = existedImage.Id;
                }
            }
        }

        private void SetDealerIdIfExist(CarActual model)
        {
            var existedDealer = _carRepository.ReturnDealerIfExist(model.Dealer);
            if (existedDealer != null)
            {
                model.Dealer = null;
                model.DealerId = existedDealer.Id;
            }
        }

        private bool IsCarModified(CarActual newCar, CarActual existedCar)
        {
            if (newCar.ConfigurationId != existedCar.ConfigurationId) return true;
            if (newCar.ExteriorColorId != existedCar.ExteriorColorId) return true;
            if (newCar.InteriorColorId != existedCar.InteriorColorId) return true;
            if (newCar.Price != existedCar.Price) return true;

            var existedCarImages = _carRepository.GetCarImagesListByCarId(existedCar.Id);
            if (newCar.CarImages.Count != existedCarImages.Count) return true;

            var existedCarImagesIds = existedCarImages.OrderBy(p => p.ImageId).Select(p => p.ImageId).ToList();
            var newCarImagesIds = newCar.CarImages.OrderBy(p => p.ImageId).Select(p => p.ImageId).ToList();
            if (!existedCarImagesIds.SequenceEqual(newCarImagesIds)) return true;

            return false;
        }
    }
}
