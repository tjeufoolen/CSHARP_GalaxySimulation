namespace FlatGalaxy.Models
{
    public abstract class AState
    {
        protected CelestialBody _context;

        public void SetContext(CelestialBody body)
        {
            this._context = body;
        }

        public abstract void OnCollision();

        public abstract AState Clone();
    }
}
