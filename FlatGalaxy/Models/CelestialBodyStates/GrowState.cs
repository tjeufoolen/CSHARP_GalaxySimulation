namespace FlatGalaxy.Models.CelestialBodyStates
{
    public class GrowState : AState
    {
        public override void OnCollision()
        {
            //grow: de radius van de entiteit groeit met 1. Op het moment dat de entiteit groter dan 20 wordt
            //      zal de entiteit veranderen door het explode gedrag aan te nemen.
            int radius = _context.Radius + 1;
            _context.Radius = radius;
            if (radius > 20) _context.State = new ExplodeState();
        }

        public override AState Clone() => new GrowState();
    }
}
