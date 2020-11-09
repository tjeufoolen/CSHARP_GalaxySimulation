using FlatGalaxy.Enums;
using FlatGalaxy.Exceptions;
using FlatGalaxy.Models;
using FlatGalaxy.Models.CelestialBodyStates;
using System.Collections.Generic;

namespace FlatGalaxy.FileHandling.Parsers
{
    public abstract class AFileParserStrategy
    {
        protected Dictionary<int, Planet> _planets = new Dictionary<int, Planet>();
        protected Dictionary<int, Asteroid> _asteroids = new Dictionary<int, Asteroid>();

        protected void Init()
        {
            _planets.Clear();
            _asteroids.Clear();
        }

        protected AState SelectState(string innerText) => innerText switch
        {
            "grow" => new GrowState(),
            "blink" => new BlinkState(),
            "bounce" => new BounceState(),
            "disappear" => new DisappearState(),
            "explode" => new ExplodeState(),
            _ => throw new TypeNotSupportedException()
        };

        protected FlatGalaxyColour SelectColour(string innerText) => innerText.ToUpper() switch
        {
            "BLACK" => FlatGalaxyColour.BLACK,
            "ORANGE" => FlatGalaxyColour.ORANGE,
            "PINK" => FlatGalaxyColour.PINK,
            "PURPLE" => FlatGalaxyColour.PURPLE,
            "BLUE" => FlatGalaxyColour.BLUE,
            "BROWN" => FlatGalaxyColour.BROWN,
            "GREY" => FlatGalaxyColour.GREY,
            "YELLOW" => FlatGalaxyColour.YELLOW,
            _ => FlatGalaxyColour.TRANSPARENT
        };
    }
}
