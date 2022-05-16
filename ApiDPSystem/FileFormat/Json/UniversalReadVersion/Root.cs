using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiDPSystem.FileFormat.Json.UniversalReadVersion
{
    public record Root
    {
        [JsonPropertyName("cars")]
        public List<Car> Cars { get; set; }
    }
}