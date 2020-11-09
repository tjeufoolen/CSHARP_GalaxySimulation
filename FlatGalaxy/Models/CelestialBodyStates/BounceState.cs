namespace FlatGalaxy.Models.CelestialBodyStates
{
    public class BounceState : AState
    {
        private int _numberOfCalls = 0;

        public BounceState()
        {
        }

        public BounceState(int numberOfCalls)
        {
            this._numberOfCalls = numberOfCalls;
        }

        public override void OnCollision()
        {
            // blink: laat de entiteit oplichten in een andere kleur als er gebotst wordt.
            if (this._numberOfCalls >= 5)
                _context.State = new BlinkState();
            else
            {
                this._numberOfCalls++;
                _context.XVelocity *= -1;
                _context.YVelocity *= -1;
            }
        }

        public override AState Clone()
        {
            return new BounceState(this._numberOfCalls);
        }
    }
}
