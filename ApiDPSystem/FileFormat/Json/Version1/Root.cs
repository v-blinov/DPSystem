using ApiDPSystem.Interfaces;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiDPSystem.FileFormat.Json.Version1
{
    public class Root : IFormat<Car>
    {
        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("cars")]
        public List<Car> Cars { get; set; }
    }
}
