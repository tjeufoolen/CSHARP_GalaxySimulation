using FlatGalaxy.Datastructures;
using FlatGalaxy.Managers;
using FlatGalaxy.Models;
using FlatGalaxy.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;

namespace FlatGalaxy.ViewModels
{
    /// <summary>
    /// 
    /// ## Concern
    /// 
    /// MvvM Pattern
    /// 
    /// The main mvvm pattern was chosen because
    /// the center of the application was based on the dataset. 
    /// Therefore it was logical to create one place where all 
    /// logic and information was stored, sorted and interacted with. 
    /// 
    /// After some collaborative thinking we decided based on previous experience that this pattern was the preferred method. 
    /// 
    /// ## Resources
    // 
    /// [Data binding and MVVM](https://docs.microsoft.com/en-us/windows/uwp/data-binding/data-binding-and-mvvm)
    /// [MvvM Light] (http://www.mvvmlight.net/)
    /// 
    /// </summary>
    public class MainVM : ViewModelBase, INotifyPropertyChanged
    {
        #region Instance variables & Properties
        private readonly INavigationService _navigationService;
        private readonly InputManager _inputManager;
        private GalaxyCaretaker _galaxyCaretaker;

        public static int CANVAS_WIDTH { get; } = 800;
        public static int CANVAS_HEIGHT { get; } = 600;

        private Task _task;

        private bool _keepRunning = false;

        public long Delay { get; set; } = 5;

        private Galaxy _galaxy;
        public Galaxy Galaxy
        {
            get => _galaxy;
            set
            {
                _galaxy = value;
                _galaxyCaretaker = new GalaxyCaretaker(_galaxy);
                UpdateView();
            }
        }

        public RangeEnabledObservableCollection<object> DrawableObjects { get; set; } = new RangeEnabledObservableCollection<object>();
        public bool IsSimulationRunning { get; private set; } = false;
        #endregion Instance variables & Properties

        public MainVM(INavigationService navigationService, InputManager inputManager)
        {
            this._navigationService = navigationService;
            this._inputManager = inputManager;

            // Set window settings
            ApplicationView.PreferredLaunchViewSize = new Windows.Foundation.Size(CANVAS_WIDTH, CANVAS_HEIGHT + 40);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += (s, e) => _keepRunning = false;

            // Set initial key bindings to commands
            this._inputManager.BindKeyToCommand("Play_Pause_Simulation", VirtualKey.K, PlayPauseSimulationCommand);
            this._inputManager.BindKeyToCommand("SpeedUp_Simulation", VirtualKey.L, SpeedUpSimulationCommand);
            this._inputManager.BindKeyToCommand("SlowDown_Simulation", VirtualKey.J, SlowDownSimulationCommand);
            this._inputManager.BindKeyToCommand("Revert_Simulation", VirtualKey.R, RevertSimulationCommand);
            this._inputManager.BindKeyToCommand("Open_Settings", VirtualKey.Escape, SettingsCommand);
            this._inputManager.BindKeyToCommand("Open_Load_Galaxy", VirtualKey.O, LoadGalaxyCommand);

            _keepRunning = true;
            StartSimulation();
        }

        private void StartSimulation()
        {
            _task = new Task(Run);
            _task.Start();
        }

        private async void Run()
        {
            Stopwatch stopwatch = new Stopwatch();

            while (_keepRunning)
            {
                if (IsSimulationRunning && _galaxy != null && _galaxyCaretaker != null)
                {
                    stopwatch.Reset();
                    stopwatch.Start();

                    _galaxyCaretaker.Backup();
                    _galaxy.Update();

                    // Run ui updates on its appropiate thread.
                    // Found solution from; https://stackoverflow.com/questions/19341591/the-application-called-an-interface-that-was-marshalled-for-a-different-thread
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        UpdateView();
                    });

                    stopwatch.Stop();
                    Thread.Sleep((int)Math.Max(0, Delay - stopwatch.ElapsedMilliseconds));
                }
            }
        }

        private void UpdateView()
        {
            DrawableObjects.Clear();

            Drawlines();
            DrawAsteroids();
            DrawPlanets();
        }

        #region Draw helpers
        private void DrawPlanets()
        {
            List<CelestialBodyVM> planets = Galaxy.CelestialBodies
                .Where(cb => cb.IsActive && cb is Planet)
                .Select(cb => new CelestialBodyVM(cb.CenterXPosition, cb.CenterYPosition, cb.FillColour, cb.Diameter))
                .ToList();
            DrawableObjects.InsertRange(planets);
        }

        private void DrawAsteroids()
        {
            List<CelestialBodyVM> asteroids = Galaxy.CelestialBodies
                .Where(cb => cb.IsActive && cb is Asteroid)
                .Select(cb => new CelestialBodyVM(cb.CenterXPosition, cb.CenterYPosition, cb.FillColour, cb.Diameter))
                .ToList();
            DrawableObjects.InsertRange(asteroids);
        }

        private void Drawlines()
        {
            Dictionary<Planet, List<Planet>> links = Galaxy.CelestialBodies
                .Where(cb => cb.IsActive && cb is Planet)
                .Select(cb => (Planet)cb)
                .ToDictionary(x => x, x => x.Neighbours);

            List<LineVM> lines = new List<LineVM>();
            links.Keys.ToList().ForEach(p => p.Neighbours.Where(p => p.IsActive).ToList().ForEach(nb =>
            {
                // Catch that we don't draw the same link multiple times
                bool found = false;
                lines.ForEach(line =>
                {
                    if (line.X1 == p.XPosition && line.Y1 == p.YPosition &&
                        line.X2 == nb.XPosition && line.Y2 == nb.YPosition)
                        found = true;
                });

                if (!found) lines.Add(new LineVM(p.XPosition, p.YPosition, nb.XPosition, nb.YPosition));
            }));

            DrawableObjects.InsertRange(lines);
        }
        #endregion Draw helpers

        #region Commands
        private ICommand _playPauseSimulationCommand;
        public ICommand PlayPauseSimulationCommand
        {
            get
            {
                if (_playPauseSimulationCommand == null)
                    _playPauseSimulationCommand = new RelayCommand(() =>
                    {
                        IsSimulationRunning = !IsSimulationRunning;
                    });

                return _playPauseSimulationCommand;
            }
        }

        private ICommand _speedUpSimulationCommand;
        public ICommand SpeedUpSimulationCommand
        {
            get
            {
                if (_speedUpSimulationCommand == null)
                    _speedUpSimulationCommand = new RelayCommand(() =>
                    {
                        if (Delay - 5 > 0) Delay -= 5;
                    });

                return _speedUpSimulationCommand;
            }
        }

        private ICommand _slowDownSimulationCommand;
        public ICommand SlowDownSimulationCommand
        {
            get
            {
                if (_slowDownSimulationCommand == null)
                    _slowDownSimulationCommand = new RelayCommand(() =>
                    {
                        if (Delay + 5 < 100) Delay += 5;
                    });

                return _slowDownSimulationCommand;
            }
        }

        private ICommand _revertSimulationCommand;
        public ICommand RevertSimulationCommand
        {
            get
            {
                if (_revertSimulationCommand == null)
                    _revertSimulationCommand = new RelayCommand(() =>
                    {
                        _galaxyCaretaker.Revert((int)(5000 / Delay) / 2); // Revert 5 seconds
                    });

                return _revertSimulationCommand;
            }
        }

        private ICommand _settingsCommand;
        public ICommand SettingsCommand
        {
            get
            {
                if (_settingsCommand == null)
                    _settingsCommand = new RelayCommand(() =>
                    {
                        IsSimulationRunning = false;
                        this._inputManager.IsEnabled = false;
                        _navigationService.Navigate(typeof(SettingsPage));
                    });

                return _settingsCommand;
            }
        }

        private ICommand _loadGalaxyCommand;
        public ICommand LoadGalaxyCommand
        {
            get
            {
                if (_loadGalaxyCommand == null)
                    _loadGalaxyCommand = new RelayCommand(() =>
                    {
                        IsSimulationRunning = false;
                        this._inputManager.IsEnabled = false;
                        _navigationService.Navigate(typeof(UploadPage));
                    });

                return _loadGalaxyCommand;
            }
        }
        #endregion Commands
    }
}
