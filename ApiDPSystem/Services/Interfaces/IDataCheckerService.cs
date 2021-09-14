using System.Collections.Generic;
using ApiDPSystem.Entities;

namespace ApiDPSystem.Services.Interfaces
{
    public interface IDataCheckerService
    {
        public void MarkSoldCars(List<Car> newCars, string dealerName);
        public void SetToDatabase(List<Car> models);
    }
}