using System.Collections.Generic;
using ApiDPSystem.Entities;

namespace ApiDPSystem.Interfaces
{
    public interface IParser
    {
        public string ConvertableFileExtension { get; }

        public List<CarActual> Parse(string fileContent, string fileName, string dealer);
    }
}