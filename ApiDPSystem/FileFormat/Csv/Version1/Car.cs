using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;

namespace ApiDPSystem.FileFormat.Csv.Version1
{
    public class Car
    {
        [Name("id")]
        public string Id { get; set; }

        [Name("year")]
        public string Year { get; set; }

        [Name("make")]
        public string Make { get; set; }

        [Name("model")]
        public string Model { get; set; }

        [Name("model trim")]
        public string ModelTrim { get; set; }

        public TechincalOptions TechincalOptions { get; set; }

        public OtherOptions OtherOptions { get; set; }

        public Colors Colors { get; set; }

        public List<string> Images { get; set; }

        [Name("price")]
        public string Price { get; set; }
    }
}
