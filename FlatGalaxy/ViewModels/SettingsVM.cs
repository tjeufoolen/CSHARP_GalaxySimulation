using FlatGalaxy.Managers;
using FlatGalaxy.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Popups;

namespace FlatGalaxy.ViewModels
{
    public class SettingsVM : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly InputManager _inputManager;

        public List<KeyBindingVM> KeyBindings { get; set; } = new List<KeyBindingVM>();

        public SettingsVM(INavigationService navigationService, InputManager inputManager)
        {
            // Init local variables
            this._navigationService = navigationService;
            this._inputManager = inputManager;

            // Get all keybindings
            this._inputManager.Bindings.Keys.ToList().ForEach(name =>
            {
                VirtualKey key = this._inputManager.Bindings[name];
                KeyBindings.Add(new KeyBindingVM(name, key.ToString()));
            });
        }

        #region Commands
        private ICommand _backCommand = null;
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                    _backCommand = new RelayCommand(() =>
                    {
                        this._inputManager.IsEnabled = true;
                        _navigationService.GoBack();
                    });

                return _backCommand;
            }
        }

        private ICommand _saveCommand = null;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(() =>
                    {
                        try
                        {
                            KeyBindings.ForEach(bindingVM =>
                            {
                                _inputManager.BindKeyToCommand(bindingVM.Name, GetKeyFromString(bindingVM.Key));
                            });

                            ShowMessageDialog("The keybindings have been saved!");
                        }

                        catch (DuplicateNameException) // a specified key is already in use
                        {
                            ShowMessageDialog("You can only use a key once. Be sure that all keys are unique!");
                        }

                        catch (KeyNotFoundException) // a specified key could not be converted
                        {
                            ShowMessageDialog("The keybindings could not be saved. Be sure that all keys actually exist!");
                        }
                    });

                return _saveCommand;
            }
        }
        #endregion

        #region Helpers
        private VirtualKey GetKeyFromString(string str)
        {
            List<string> vkn = Enum.GetNames(typeof(VirtualKey)).ToList();
            string vks = vkn.FirstOrDefault(vk => vk.ToUpper().Equals(str.ToUpper()));
            if (vks == null) throw new KeyNotFoundException();
            return (VirtualKey)Enum.Parse(typeof(VirtualKey), vks);
        }

        private async void ShowMessageDialog(string message) => await new MessageDialog(message).ShowAsync();
        #endregion Helpers
    }
}
