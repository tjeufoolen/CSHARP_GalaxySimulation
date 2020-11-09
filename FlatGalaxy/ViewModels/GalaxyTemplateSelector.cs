using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FlatGalaxy.ViewModels
{
    /// <summary>
    /// 
    /// ## Concern
    /// 
    /// GalaxyTemplateSelector
    ///
    /// Template selector for identifying how to draw an object to the canvas.
    /// 
    /// ## Resources
    /// 
    /// [Stackoverflow post about the implementation](https://stackoverflow.com/questions/59986975/true-alternative-of-compositecollection-for-combobox-in-uwp)
    /// 
    /// </summary>
    public class GalaxyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CelestialBodyTemplate { get; set; }
        public DataTemplate LineTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item) => GetDataTemplate(item);

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) => GetDataTemplate(item);

        private DataTemplate GetDataTemplate(object item) => (item is LineVM) ? LineTemplate : CelestialBodyTemplate;
    }
}
