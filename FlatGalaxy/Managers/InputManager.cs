using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace FlatGalaxy.Managers
{
    /// <summary>
    /// 
    /// ## Concern
    /// 
    /// Input Manager, for binding all commands to the correct keys. 
    /// 
    /// The BindKeyToCommand method can be run on 2 different occassions.
    /// - When you want to bind a key to a command that hasn't been linked before.
    /// - When you want to rebind a key to a command.
    /// 
    /// </summary>
    public class InputManager
    {
        public bool IsEnabled { get; set; } = true; // Possibility to toggle listening for keys and executing commands
        public Dictionary<string, VirtualKey> Bindings { get; private set; }
        public Dictionary<VirtualKey, ICommand> Actions { get; private set; }

        public InputManager()
        {
            Bindings = new Dictionary<string, VirtualKey>();
            Actions = new Dictionary<VirtualKey, ICommand>();

            Window.Current.CoreWindow.KeyDown += (window, e) =>
            {
                if (IsEnabled) HandleKeyInput(e);
            };
        }

        public void BindKeyToCommand(string name, VirtualKey key, ICommand command = null)
        {
            name = name.ToUpper();

            if (command == null) // If no command  was specified, it means that it already exists and we should rebind it.
            {
                command = Actions[Bindings[name]];
                Actions.Remove(Bindings[name]);
            }

            if (Actions.ContainsKey(key) && Actions[key] != command) throw new DuplicateNameException();

            Bindings[name] = key;
            Actions[key] = command;
        }

        public VirtualKey GetKeyFromBinding(string name) => Bindings.FirstOrDefault(b => b.Key == name.ToUpper()).Value;

        private void HandleKeyInput(KeyEventArgs e)
        {
            VirtualKey key = e.VirtualKey;

            if (Actions.ContainsKey(key))
                if (Actions[key].CanExecute(null))
                    Actions[key].Execute(null);
        }
    }
}
