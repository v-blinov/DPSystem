using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiDPSystem.Json.Version1
{
    public class Root
    {
        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("cars")]
        public List<Car> Cars { get; set; }
    }
}
