using System.Collections.Generic;

namespace ApiDPSystem.Interfaces
{
    public interface IFormat<T>
    {
        public int Version { get; set; }
        public List<T> Cars { get; set; }
    }
}
