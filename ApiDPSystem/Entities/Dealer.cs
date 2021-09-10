using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Dealer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Car> Car { get; set; }


        public override bool Equals(object obj)
        {
            if (obj is not Dealer dealer)
                return false;

            return dealer.Name == Name;
        }

        public override int GetHashCode() =>
            Name.GetHashCode() * 31;
    }
}