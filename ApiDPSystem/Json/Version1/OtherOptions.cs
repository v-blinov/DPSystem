using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiDPSystem.Json.Version1
{
    public class OtherOptions
    {
        [JsonPropertyName("exterior")]
        public object Exterior { get; set; }

        [JsonPropertyName("interior")]
        public List<string> Interior { get; set; }

        [JsonPropertyName("safety")]
        public List<string> Safety { get; set; }
    }
}
