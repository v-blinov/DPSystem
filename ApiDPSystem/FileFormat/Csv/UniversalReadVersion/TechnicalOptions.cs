using CsvHelper.Configuration.Attributes;

namespace ApiDPSystem.FileFormat.Csv.UniversalReadVersion
{
    public record TechnicalOptions
    {
        [Name("techincal options/engine")]
        public Engine Engine { get; set; }

        [Name("techincal options/transmission")]
        public string Transmission { get; set; }

        [Name("techincal options/drive")]
        public string Drive { get; set; }
    }
}