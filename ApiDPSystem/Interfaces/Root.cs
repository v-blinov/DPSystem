using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiDPSystem.Interfaces
{
    public class Root<T>
    {
        [JsonPropertyName("cars")]
        public List<T> Cars { get; set; }
    }
}
