using ApiDPSystem.FileFormat;
using System.Collections.Generic;

namespace ApiDPSystem.Interfaces
{
    public interface IBParser
    {
        public string ConvertableFileExtension { get; }

        //public int GetVersion()
        public List<Entities.CarActual> Parse(string fileContent, string fileName, string dealer);
    }
}
