using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Producer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Configuration> Configurations { get; set; }


        public override bool Equals(object obj)
        {
            if (obj is not Producer producer)
                return false;

            return producer.Name == Name;
        }

        public override int GetHashCode() =>
            Name.GetHashCode() * 22;

        public Producer GetValuesCopy() =>
            new()
            {
                Name = Name
            };
    }
}