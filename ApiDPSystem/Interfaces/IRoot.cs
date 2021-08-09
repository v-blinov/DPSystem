using System.Collections.Generic;
using ApiDPSystem.Entities;

namespace ApiDPSystem.Interfaces
{
    public interface IRoot
    {
        public int Version { get; }
        public string FileFormat { get; }

        public List<CarActual> ConvertToActualDbModel(string dealerName);
    }
}