namespace FlatGalaxy.Models.CelestialBodyStates
{
    public class DisappearState : AState
    {
        private bool _collided = false;
        public bool Collided
        {
            get => _collided;
            set
            {
                _collided = value;
                if (this._context != null) this._context.IsActive = !_collided;
            }
        }

        public override void OnCollision()
        {
            //disappear: de entiteit verdwijnt/gaat kapot (en wordt dus ook uit memory verwijderd).
            //Denk dat je gewoon de deconstructor aan kan maken/aanroepen in celestialbody. maar die mag nooit public zijn. 
            Collided = true;
        }

        public override AState Clone()
        {
            return new DisappearState() { Collided = this.Collided };
        }
    }
}
