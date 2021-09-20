using System.Collections.Generic;
using ApiDPSystem.Entities;

namespace ApiDPSystem.Interfaces
{
    public interface IParser
    {
        public string ConvertableFileExtension { get; }
        public List<Car> Parse(string fileContent, string fileName, string dealer);
    }
}