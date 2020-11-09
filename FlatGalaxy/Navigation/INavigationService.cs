using System;

namespace FlatGalaxy.Navigation
{
    public interface INavigationService
    {
        void Navigate(Type sourcePage);
        void Navigate(Type sourcePage, object parameter);
        void GoBack();
    }
}
