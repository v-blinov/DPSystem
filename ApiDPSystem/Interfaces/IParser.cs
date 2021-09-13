using ApiDPSystem.Entities;
using System.Collections.Generic;

namespace ApiDPSystem.Interfaces
{
    public interface IParser
    {
        public string ConvertableFileExtension { get; }
        public List<Car> Parse(string fileContent, string fileName, string dealer);
    }
}