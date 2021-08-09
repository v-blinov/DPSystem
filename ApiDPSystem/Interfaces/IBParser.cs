using System.Collections.Generic;
using ApiDPSystem.Entities;

namespace ApiDPSystem.Interfaces
{
    public interface IBParser
    {
        public string ConvertableFileExtension { get; }

        //public int GetVersion()
        public List<CarActual> Parse(string fileContent, string fileName, string dealer);
    }
}