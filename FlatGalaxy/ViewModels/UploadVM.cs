using CommonServiceLocator;
using FlatGalaxy.Exceptions;
using FlatGalaxy.Managers;
using FlatGalaxy.Models;
using FlatGalaxy.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.IO;
using System.Net;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace FlatGalaxy.ViewModels
{
    public class UploadVM : ViewModelBase
    {
        #region Instance variables & Properties
        private readonly INavigationService _navigationService;
        private readonly FileManager _fileManager;
        private readonly InputManager _inputManager;

        public string FileUrl { get; set; } = "";
        #endregion Instance variables & Properties

        public UploadVM(INavigationService navigationService, FileManager fileManager, InputManager inputManager)
        {
            this._navigationService = navigationService;
            this._fileManager = fileManager;
            this._inputManager = inputManager;
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

        private ICommand _openOnlineFileCommand = null;
        public ICommand OpenOnlineFileCommand
        {
            get
            {
                if (_openOnlineFileCommand == null)
                    _openOnlineFileCommand = new RelayCommand(() => PickOnlineFile());

                return _openOnlineFileCommand;
            }
        }

        private ICommand _openOfflineFileCommand = null;
        public ICommand OpenOfflineFileCommand
        {
            get
            {
                if (_openOfflineFileCommand == null)
                    _openOfflineFileCommand = new RelayCommand(() => PickLocalFile());

                return _openOfflineFileCommand;
            }
        }
        #endregion

        private async void ShowMessageDialog(string message) => await new MessageDialog(message).ShowAsync();

        private void SetGalaxy(Galaxy galaxy)
        {
            ServiceLocator.Current.GetInstance<MainVM>().Galaxy = galaxy;
            this._inputManager.IsEnabled = true;
            this.FileUrl = "";
            _navigationService.Navigate(typeof(MainPage));
        }

        private void PickOnlineFile()
        {
            if (FileUrl.Length > 0)
            {
                try
                {
                    SetGalaxy(_fileManager.LoadOnline(FileUrl));
                }

                catch (UriFormatException) // url was misformed
                {
                    ShowMessageDialog("It looks like the url wasn't correctly formatted, please try again!");
                }

                catch (FileTypeNotSupportedException) // file is not of type csv or xml
                {
                    ShowMessageDialog("The specified file type is not supported. Please provide a valid url that points to a csv or xml file.");
                }

                catch (Exception ex)
                {
                    if (ex is WebException || ex is FileLoadException) // url was not valid -or- url does not point to a file
                        ShowMessageDialog("No file could be found using the provided url. Please provide a valid url that points to a file.");
                }
            }
        }

        private async void PickLocalFile()
        {
            try
            {
                var picker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.List,
                    SuggestedStartLocation = PickerLocationId.Downloads
                };
                picker.FileTypeFilter.Add(".csv");
                picker.FileTypeFilter.Add(".xml");

                StorageFile file = await picker.PickSingleFileAsync();
                SetGalaxy(_fileManager.LoadOffline(file));
            }

            catch (FileNotSelectedException) { } // Picker was closed without selecting a file
        }
    }
}
