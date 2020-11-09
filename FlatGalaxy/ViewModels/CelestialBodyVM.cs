namespace FlatGalaxy.ViewModels
{
    public class CelestialBodyVM
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public string Colour { get; private set; }
        public int Diameter { get; private set; }

        public CelestialBodyVM(double x, double y, string colour, int diameter)
        {
            this.X = x;
            this.Y = y;
            this.Colour = colour;
            this.Diameter = diameter;
        }
    }
}
