using System.Text.Json.Serialization;

namespace ApiDPSystem.FileFormat.Json.Version1
{
    public class Engine
    {
        [JsonPropertyName("fuel")]
        public string Fuel { get; set; }

        [JsonPropertyName("power")]
        public string Power { get; set; }

        [JsonPropertyName("capacity")]
        public string Capacity { get; set; }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is not Engine engine)
                return false;

            return engine.Fuel == Fuel && engine.Power == Power && engine.Capacity == Capacity;
        }

        public bool Equals(Engine obj)
        {
            if (obj == null)
                return false;

            return obj.Fuel == Fuel && obj.Power == Power && obj.Capacity == Capacity;
        }
    }
}