using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace ApiDPSystem.FileFormat.Csv.UniversalReadVersion
{
    public record Root()
    {
        [Name("cars")]
        public List<Car> Cars { get; set; }
    }
}