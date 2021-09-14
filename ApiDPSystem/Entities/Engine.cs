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
            if (obj is not Engine engine)
                return false;

            var existedEngine = engine.Fuel == Fuel &&
                                engine.Capacity == Capacity &&
                                engine.Power == Power;

            return existedEngine;
        }

        public override int GetHashCode() =>
            Fuel.GetHashCode() * 22 + Capacity.GetHashCode() * 13 + Power.GetHashCode() * 7;

        public Engine GetValuesCopy() =>
            new()
            {
                Fuel = Fuel,
                Power = Power,
                Capacity = Capacity
            };
    }
}