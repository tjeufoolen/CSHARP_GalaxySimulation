using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FlatGalaxy.Datastructures
{
    /// <summary>
    /// 
    /// ## Concern
    /// 
    /// RangeEnabledObservableCollection
    ///
    /// A default observablecollection with an additional InsertRange method which only sends 
    /// a NotifiyCollectionChangedEvent after everything has been added, instead of the default Insert()
    /// which notifies on each insert.
    /// 
    /// ## Resources
    /// 
    /// [Stackoverflow post about the implementation](https://stackoverflow.com/questions/8606994/adding-a-range-of-values-to-an-observablecollection-efficiently)
    /// 
    /// </summary>
    public class RangeEnabledObservableCollection<T> : ObservableCollection<T>
    {
        public void InsertRange(IEnumerable<T> items)
        {
            this.CheckReentrancy();
            foreach (var item in items)
                this.Items.Add(item);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
