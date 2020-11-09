using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FlatGalaxy.Navigation
{
    /// <summary>
    /// 
    /// ## Concern
    /// 
    /// Navigation service
    ///
    /// The navigaion service to create a clean way for the viewmodel to change pages.
    /// 
    /// ## Resources
    /// 
    /// [Stackoverflow post about the implementation](https://stackoverflow.com/questions/26816264/page-navigation-using-mvvm-in-store-app)
    /// 
    /// </summary>
    public class NavigationService : INavigationService
    {
        public void Navigate(Type sourcePage)
        {
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(sourcePage);
        }

        public void Navigate(Type sourcePage, object parameter)
        {
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(sourcePage, parameter);
        }

        public void GoBack()
        {
            var frame = (Frame)Window.Current.Content;
            frame.GoBack();
        }
    }
}
