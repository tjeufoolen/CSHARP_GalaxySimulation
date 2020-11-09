using FlatGalaxy.Enums;

namespace FlatGalaxy.Models.CelestialBodyStates
{
    public class BlinkState : AState
    {
        public override void OnCollision()
        {
            //blink: laat de entiteit oplichten in een andere kleur als er gebotst wordt.
            _context.HighlightColour = FlatGalaxyColour.GREEN;
        }

        public override AState Clone() => new BlinkState();
    }
}
