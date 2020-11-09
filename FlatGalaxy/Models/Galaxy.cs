using FlatGalaxy.Models.CelestialBodyStates;
using FlatGalaxy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatGalaxy.Models
{
    public class Galaxy
    {
        public List<CelestialBody> CelestialBodies { get; private set; }

        public Galaxy(List<CelestialBody> celestialBodies)
        {
            this.CelestialBodies = celestialBodies;
        }

        public void Update()
        {
            this.CelestialBodies.Where(cb => cb.IsActive).ToList().ForEach(cb =>
            {
                // Move celestialbodies
                cb.Move(MainVM.CANVAS_WIDTH, MainVM.CANVAS_HEIGHT);

                // Check for collision
                HandleCollision(cb);
            });
        }

        private void HandleCollision(CelestialBody cb)
        {
            this.CelestialBodies.Where(cb => cb.IsActive).ToList().ForEach(other =>
            {
                if (cb != other && cb.Intersects(other))
                {
                    cb.State.OnCollision();
                    other.State.OnCollision();

                    if (cb.State is ExplodeState) this.HandleExplodeState(cb);
                    if (other.State is ExplodeState) this.HandleExplodeState(other);
                }
            });
        }

        private void HandleExplodeState(CelestialBody cb)
        {
            ExplodeState state = ((ExplodeState)cb.State);

            if (!state.Exploded)
            {
                for (int i = 0; i < ExplodeState.SPAWN_AMOUNT; i++)
                {
                    double xVelocity = GetRandomDoubleBetween(-3, 3);
                    double yVelocity = GetRandomDoubleBetween(-3, 3);
                    this.CelestialBodies.Add(new Asteroid(cb.XPosition, cb.YPosition, cb.Radius / 2, xVelocity, yVelocity, new BounceState()));
                }
                state.Exploded = true;
            }
        }

        public IGalaxyMemento Save() => new GalaxyMemento(this.CelestialBodies.Where(cb => cb.IsActive).ToList());

        public void Restore(IGalaxyMemento previousState)
        {
            if (!(previousState is GalaxyMemento)) throw new Exception("Unknown memento");
            this.CelestialBodies = previousState.CelestialBodies;
        }

        #region Helpers
        private double GetRandomDoubleBetween(double min, double max) => new Random().NextDouble() * (max - min) + min;
        #endregion
    }
}
