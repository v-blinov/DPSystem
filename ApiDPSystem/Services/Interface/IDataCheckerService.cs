using ApiDPSystem.Entities;
using System.Collections.Generic;

namespace ApiDPSystem.Services.Interface
{
    public interface IDataCheckerService
    {
        public void TransferSoldCars(List<CarActual> newListCars, string dealerName);
        public void SetToDatabase(List<CarActual> models);
    }
}
