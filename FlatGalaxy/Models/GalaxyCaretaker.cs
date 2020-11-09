using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatGalaxy.Models
{
    public class GalaxyCaretaker
    {
        private const int MAX_MEMENTO_SIZE = 10000;
        public Galaxy Galaxy { get; private set; }
        public List<IGalaxyMemento> GalaxyMementos { get; set; } = new List<IGalaxyMemento>();

        public GalaxyCaretaker(Galaxy galaxy)
        {
            this.Galaxy = galaxy;
        }

        public void Backup()
        {
            IGalaxyMemento memento = Galaxy.Save();
            this.GalaxyMementos.Add(memento);
            this.LimitMementoSize();
        }

        private void LimitMementoSize()
        {
            if (this.GalaxyMementos.Count > MAX_MEMENTO_SIZE)
                this.GalaxyMementos.RemoveRange(0, this.GalaxyMementos.Count - MAX_MEMENTO_SIZE);
        }

        private void Revert()
        {
            if (this.GalaxyMementos.Count == 0) return;

            IGalaxyMemento memento = this.GalaxyMementos.Last();
            this.GalaxyMementos.Remove(memento);
            try
            {
                this.Galaxy.Restore(memento);
            }
            catch (Exception)
            {
                this.Revert();
            }
        }

        public void Revert(int steps)
        {
            for (int i = 0; i < steps; i++) this.Revert();
        }
    }
}
