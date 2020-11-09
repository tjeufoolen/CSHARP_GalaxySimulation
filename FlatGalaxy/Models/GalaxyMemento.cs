using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatGalaxy.Models
{
    public class GalaxyMemento : IGalaxyMemento
    {
        public long TimeStamp { get; set; }
        public List<CelestialBody> CelestialBodies { get; set; }

        public GalaxyMemento(List<CelestialBody> celestialBodies)
        {
            this.CelestialBodies = new List<CelestialBody>();
            celestialBodies.ForEach(cb => CelestialBodies.Add((CelestialBody)cb.Clone()));
            celestialBodies.Where(cb => cb is Planet).Select(cb => ((Planet)cb)).ToList().ForEach(oldPlanet =>
           {
               oldPlanet.Neighbours.ForEach(oldNeighbour =>
               {
                   var planets = this.CelestialBodies
                       .Where(cb => cb is Planet)
                       .Select(cb => ((Planet)cb))
                       .ToList();

                   planets.FirstOrDefault(p => p.Name == oldPlanet.Name).AddNeighbour(planets.FirstOrDefault(p => p.Name == oldNeighbour.Name));
               });
           });

            this.TimeStamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
