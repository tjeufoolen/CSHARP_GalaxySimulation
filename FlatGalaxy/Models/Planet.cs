using FlatGalaxy.Enums;
using System.Collections.Generic;

namespace FlatGalaxy.Models
{
    public class Planet : CelestialBody
    {
        public string Name { get; private set; }
        public List<Planet> Neighbours { get; private set; }

        public Planet(string name, double xPosition, double yPosition, int radius, FlatGalaxyColour colour, double xVelocity, double yVelocity, AState state) : base(xPosition, yPosition, radius, colour, xVelocity, yVelocity, state)
        {
            this.Name = name;
            this.Neighbours = new List<Planet>();
        }

        public void AddNeighbour(Planet neighbour) => Neighbours.Add(neighbour);

        public override CelestialBody Clone() // Disclaimer; neighbours should be added later
        {
            return new Planet(this.Name, this.XPosition, this.YPosition, this.Radius, this.Colour, this.XVelocity, this.YVelocity, this.State.Clone());
        }
    }
}
