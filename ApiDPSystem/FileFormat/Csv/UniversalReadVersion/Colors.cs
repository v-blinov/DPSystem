using CsvHelper.Configuration.Attributes;

namespace ApiDPSystem.FileFormat.Csv.UniversalReadVersion
{
    public record Colors()
    {
        [Name("colors/interior")]
        public string Interior { get; set; }

        [Name("colors/exterior")]
        public string Exterior { get; set; }
    }
}