using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Dealer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CarActual> CarActuals { get; set; }
        public ICollection<CarHistory> CarHistories { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is not Dealer dealer)
                return false;

            return dealer.Name == Name;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() * 31;
        }
    }
}
