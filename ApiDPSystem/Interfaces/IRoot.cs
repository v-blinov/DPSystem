using ApiDPSystem.FileFormat;
using System.Collections.Generic;

namespace ApiDPSystem.Interfaces
{
    public interface IRoot
    {
        public int Version { get; }
        public string FileFormat { get; }

        public List<Entities.CarActual> ConvertToActualDbModel(string dealerName);
    }
}