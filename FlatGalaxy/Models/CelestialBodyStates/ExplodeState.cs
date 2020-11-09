namespace FlatGalaxy.Models.CelestialBodyStates
{
    public class ExplodeState : DisappearState
    {
        public static int SPAWN_AMOUNT = 5;
        public bool Exploded { get; set; } = false;

        public override void OnCollision()
        {
            //explode: de entiteit spat uiteen in 5 andere entiteiten. Deze nieuwe entiteiten krijgen allemaal
            //         de gedragseigenschap bounce(de oude entiteit zal net als disappear uit memory verwijderd
            //         worden).
            base.OnCollision();
        }

        public override AState Clone() => new ExplodeState();
    }
}
