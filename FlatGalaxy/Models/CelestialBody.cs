using FlatGalaxy.Enums;
using System;

namespace FlatGalaxy.Models
{
    public abstract class CelestialBody
    {
        #region Properties
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public int Radius { get; set; }
        protected FlatGalaxyColour Colour { get; set; }
        public double XVelocity { get; set; }
        public double YVelocity { get; set; }

        private AState _state;
        public AState State
        {
            get => _state;
            set
            {
                _state = value;
                _state.SetContext(this);
            }
        }

        public FlatGalaxyColour HighlightColour { get; set; }
        public double CenterXPosition { get => XPosition - Radius; }
        public double CenterYPosition { get => YPosition - Radius; }
        public int Diameter { get => Radius * 2; }

        public string FillColour
        {
            get
            {
                if (HighlightColour != 0)
                {
                    string temp = HighlightColour.ToHexColor();
                    HighlightColour = 0;
                    return temp;
                }
                return Colour.ToHexColor();
            }
        }

        public bool IsActive { get; set; } = true;
        #endregion Properties

        public CelestialBody(double xPosition, double yPosition, int radius, FlatGalaxyColour colour, double xVelocity, double yVelocity, AState state)
        {
            this.XPosition = xPosition;
            this.YPosition = yPosition;
            this.Radius = radius;
            this.Colour = colour;
            this.XVelocity = xVelocity;
            this.YVelocity = yVelocity;
            this.State = state;
        }

        public void Move(int boundsX, int boundsY)
        {
            // Check Bounds and invert direction if necessary
            if (this.XPosition < this.Radius || this.XPosition > boundsX - this.Radius)  // Left, Right
                this.XVelocity *= -1;
            if (this.YPosition < this.Radius || this.YPosition > boundsY - this.Radius)  // Top,  Bottom
                this.YVelocity *= -1;

            // Move
            this.XPosition += this.XVelocity;
            this.YPosition += this.YVelocity;
        }

        public bool Intersects(CelestialBody other)
        {
            var distance = Math.Sqrt(Math.Pow((this.XPosition - other.XPosition), 2) + Math.Pow((this.YPosition - other.YPosition), 2));
            return distance < this.Radius + other.Radius;
        }

        public abstract CelestialBody Clone();
    }
}
