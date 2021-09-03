using ApiDPSystem.Entities;
using System;
using System.Collections.Generic;

namespace ApiDPSystem.Repository.Interfaces
{
    public interface ICarRepository
    {
        public Configuration ReturnConfigurationIfExist(Configuration configuration);
        public Producer ReturnProducerIfExist(Producer producer);
        public Engine ReturnEngineIfExist(Engine engine);
        public Feature ReturnFeatureIfExist(Feature feature);
        public Color ReturnColorIfExist(Color color);
        public Image ReturnImageIfExist(Image image);
        public Dealer ReturnDealerIfExist(Dealer dealer);
        public CarActual GetThatDealerCarIfExist(CarActual model);
        public List<string> GetActualCarsVinCodesForDealer(string dealerName);
        public CarActual GetCar(string vincode, string dealerName);
        public void TransferOneCarFromActualToHistory(CarActual car, bool isSold = false);
        public void AddCarToDb(CarActual model);
        public List<CarImage> GetCarImagesListByCarId(Guid carId);
    }
}
