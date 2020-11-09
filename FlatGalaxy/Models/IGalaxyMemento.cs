using System.Collections.Generic;

namespace FlatGalaxy.Models
{
    public interface IGalaxyMemento
    {
        public long TimeStamp { get; set; }
        public List<CelestialBody> CelestialBodies { get; set; }
    }
}
