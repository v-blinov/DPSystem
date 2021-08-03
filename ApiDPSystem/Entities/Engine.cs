using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Engine
    {
        public int Id { get; set; }
        public string Fuel { get; set; }
        public int? Power { get; set; }
        public double? Capacity { get; set; }

        public ICollection<Configuration> Configurations { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is not Engine engine)
                return false;

            return engine.Fuel == Fuel &&
                   engine.Capacity == Capacity &&
                   engine.Power == Power;
        }
        public override int GetHashCode()
        {
            return Fuel.GetHashCode() * 22 + Capacity.GetHashCode() * 13 + Power.GetHashCode() * 7;
        }
    }
}
