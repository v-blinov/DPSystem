using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace ApiDPSystem.FileFormat.Csv.UniversalReadVersion
{
    public record OtherOptions
    {
        [Name("other options/exterior")]
        public List<string> Exterior { get; set; }

        [Name("other options/interior")]
        public List<string> Interior { get; set; }

        [Name("other options/multimedia")]
        public List<string> Multimedia { get; set; }

        [Name("other options/safety")]
        public List<string> Safety { get; set; }
    }
}