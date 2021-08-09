using CsvHelper.Configuration.Attributes;

namespace ApiDPSystem.FileFormat.Csv.Version1
{
    public class Colors
    {
        [Name("colors/interior")]
        public string Interior { get; set; }

        [Name("colors/exterior")]
        public string Exterior { get; set; }
    }
}