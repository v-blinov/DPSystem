using CsvHelper.Configuration.Attributes;

namespace ApiDPSystem.FileFormat.Csv.Version1
{
    public class TechincalOptions
    {
        public Engine Engine { get; set; }

        [Name("techincal options/transmission")]
        public string Transmission { get; set; }

        [Name("techincal options/drive")]
        public string Drive { get; set; }
    }
}