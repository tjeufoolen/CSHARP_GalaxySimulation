using FlatGalaxy.Enums;

namespace FlatGalaxy.Models
{
    public class Asteroid : CelestialBody
    {
        public Asteroid(double xPosition, double yPosition, int radius, double xVelocity, double yVelocity, AState state) : base(xPosition, yPosition, radius, FlatGalaxyColour.BLACK, xVelocity, yVelocity, state)
        {
        }

        public override CelestialBody Clone()
        {
            return new Asteroid(this.XPosition, this.YPosition, this.Radius, this.XVelocity, this.YVelocity, this.State.Clone());
        }
    }
}
