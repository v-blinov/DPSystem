using CsvHelper.Configuration.Attributes;

namespace ApiDPSystem.FileFormat.Csv.Version2
{
    public class Engine
    {
        [Name("techincal options/engine/fuel")]
        public string Fuel { get; set; }

        [Name("techincal options/engine/power")]
        public string Power { get; set; }

        [Name("techincal options/engine/capacity")]
        public string Capacity { get; set; }
    }
}
