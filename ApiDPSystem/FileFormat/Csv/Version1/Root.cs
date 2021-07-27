using ApiDPSystem.Interfaces;
using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;

namespace ApiDPSystem.FileFormat.Csv.Version1
{
    public class Root : IFormat<Car>
    {
        [Name("version")]
        public int Version { get; set; }

        public List<Car> Cars { get; set; }
    }
}
