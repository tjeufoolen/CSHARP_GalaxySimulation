using CommonServiceLocator;
using FlatGalaxy.Managers;
using FlatGalaxy.Navigation;
using GalaSoft.MvvmLight.Ioc;

namespace FlatGalaxy.ViewModels
{
    /// <summary>
    /// 
    /// ## Concern
    /// 
    /// Viewmodel locator
    /// 
    /// ## Resources
    // 
    /// [Data binding and MVVM](https://docs.microsoft.com/en-us/windows/uwp/data-binding/data-binding-and-mvvm)
    /// [MvvM Light] (http://www.mvvmlight.net/)
    /// [What is a ViewModelLocator and what are its pros/cons compared to DataTemplates?](https://stackoverflow.com/questions/5462040/what-is-a-viewmodellocator-and-what-are-its-pros-cons-compared-to-datatemplates)
    /// [Basic Example](https://github.com/qmatteoq/UWP-MVVMSamples/blob/master/MVVM%20Light/MVVMLight.Messages/ViewModels/ViewModelLocator.cs)
    /// 
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<INavigationService, NavigationService>();

            SimpleIoc.Default.Register<FileManager>();
            SimpleIoc.Default.Register<InputManager>();
            SimpleIoc.Default.Register<MainVM>();
            SimpleIoc.Default.Register<UploadVM>();
        }

        public MainVM Main
        {
            get => ServiceLocator.Current.GetInstance<MainVM>();
        }

        public UploadVM Upload
        {
            get => ServiceLocator.Current.GetInstance<UploadVM>();
        }

        public SettingsVM Settings
        {
            get => new SettingsVM(
                ServiceLocator.Current.GetInstance<INavigationService>(),
                ServiceLocator.Current.GetInstance<InputManager>()
            );
        }
    }
}
