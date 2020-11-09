namespace FlatGalaxy.ViewModels
{
    public class KeyBindingVM
    {
        public string Name { get; set; }
        public string Key { get; set; }

        public KeyBindingVM(string name, string key)
        {
            this.Name = name;
            this.Key = key;
        }
    }
}
