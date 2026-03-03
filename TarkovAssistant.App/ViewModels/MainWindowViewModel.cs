using System.Windows;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TarkovAssistant.App.Messages;
using TarkovAssistant.App.Models;
using TarkovAssistant.App.Views;
using TarkovAssistant.Domain;
using TarkovAssistant.Services;

namespace TarkovAssistant.App.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject, IDisposable
    {
        private IAppService _appService;
        private readonly IWebApiService _webApiService;
        private readonly IFileMonitor _fileMonitor;

        private readonly FormWrapper _storedForm;
        private TaskCompletionSource<bool> _loaded = new TaskCompletionSource<bool>();

        #region <Properties>

        [ObservableProperty]
        private FormModel _formInfo;

        [ObservableProperty]
        private InteractiveMapModel _interactiveMap;

        [ObservableProperty]
        private OptionsModel _options;

        [ObservableProperty]
        private bool _isFilterPanelVisible;

        [ObservableProperty]
        private bool _isMapSelectionPanelVisible;

        [ObservableProperty]
        private bool _isMarkerInfoPanelVisible;

        [ObservableProperty]
        private bool _isSettingsPanelVisible;

        [ObservableProperty]
        private MapModel? _selectedMap = null;

        partial void OnSelectedMapChanged(MapModel? value)
        {
            IsMapSelectionPanelVisible = false;
            if (value != null)
                _ = OpenSelectedMap(value);
        }

        [ObservableProperty]
        private ObservableCollection<MapModel> _maps = new();

        [ObservableProperty]
        private bool _isTestMode = false;

        #endregion

        public MainWindowViewModel(IAppService appService, IWebApiService webApiService, IFileMonitor fileMonitor)
        {
            _appService = appService;
            _webApiService = webApiService;
            _fileMonitor = fileMonitor;
            _formInfo = new FormModel();
            _storedForm = new FormWrapper(_formInfo);
            _interactiveMap = new InteractiveMapModel(appService, webApiService, fileMonitor);
            _options = new OptionsModel(appService, webApiService);
            _options.OptionsChanged += OnOptionsChanged;
#if DEBUG
            _isTestMode = true;
#else
            _isTestMode = false;
#endif

            MarkerResourceModel.DataPath = ""; // appService.Options.DataPath;
        }

        public async Task InitializeAsync()
        {
            await LoadMaps();
            _loaded.SetResult(true);
        }

        #region <Commands>

        [RelayCommand]
        private async Task SelectMap()
        {
            await _loaded.Task; // wait loading maps

            IsMarkerInfoPanelVisible = false;
            if (IsMapSelectionPanelVisible)
            {
                IsMapSelectionPanelVisible = false;
            }
            else
            {
                SelectedMap = null;
                IsMapSelectionPanelVisible = true;
            }
        }

        private async Task OpenSelectedMap(MapModel map)
        {
            IsMapSelectionPanelVisible = false;

            await InteractiveMap.Open(map);
            CenterMap();
        }

        [RelayCommand]
        private void ToggleFullScreen()
        {
            HideAllPanels();
            _storedForm.IsFullScreen = !_storedForm.IsFullScreen;
        }

        [RelayCommand]
        private void ZoomIn()
        {
            HideAllPanels();
            InteractiveMap.ZoomIn();
        }

        [RelayCommand]
        private void ZoomOut()
        {
            HideAllPanels();
            InteractiveMap.ZoomOut();
        }

        [RelayCommand]
        private void CenterMap()
        {
            HideAllPanels();
            InteractiveMap.CenterMap(FormInfo);
        }

        [RelayCommand]
        private void ToggleMapFilters()
        {
            IsMapSelectionPanelVisible = false;
            IsMarkerInfoPanelVisible = false;
            IsFilterPanelVisible = !IsFilterPanelVisible;
        }

        [RelayCommand]
        private void OpenSettings()
        {
            HideAllPanels();
            IsSettingsPanelVisible = !IsSettingsPanelVisible;
        }

        [RelayCommand]
        private async Task MarkerClick(MarkerModel marker)
        {
            HideAllPanels();
            IsMarkerInfoPanelVisible = await InteractiveMap.OpenMarker(marker.Id);
            InteractiveMap.CurrentMarker!.FinishedChanged += (value) =>
            {
                marker.IsFinished = value;                
            };
        }

        [RelayCommand]
        private void HideMarkerPanel()
        {
            IsMarkerInfoPanelVisible = false;
        }

        [RelayCommand]
        private void HideSettingsPanel()
        {
            IsSettingsPanelVisible = false;
        }

        [RelayCommand]
        private void HideAllPanels()
        {
            IsMapSelectionPanelVisible = false;
            IsFilterPanelVisible = false;
            IsMarkerInfoPanelVisible = false;
            IsSettingsPanelVisible = false;
        }

        [RelayCommand]
        private async Task AddProfile()
        {
            //var profile = new ProfileEntity();
            //profile.Name = "New Profile";
            //profile.Kind = ProfileKind.Bear;

            //var dialog = new ProfileWindow
            //{
            //    DataContext = new ProfileWindowViewModel(profile)
            //};
            //dialog.Owner = Application.Current.MainWindow;
            //WeakReferenceMessenger.Default.Register<CloseDialogMessage>(dialog, (r, m) =>
            //{
            //    dialog.DialogResult = m.Result;
            //});
            //try
            //{
            //    if (dialog.ShowDialog() ?? false)
            //    {
            //        await _profileService.AddProfileAsync(profile);
            //        var item = new ProfileModel(profile);
            //        Options.Profiles.Add(item);
            //        Options.CurrentProfile = item;

            //        _appService.Options.Profile = item.Id;
            //        await _appService.SaveOptionsAsync();
            //    }
            //}
            //finally
            //{
            //    WeakReferenceMessenger.Default.UnregisterAll(dialog);
            //}
        }

        [RelayCommand]
        private async Task EditProfile(ProfileModel profile)
        {
            //if (profile == null)
            //    return;

            //var newProfile = new ProfileEntity();
            //newProfile.Id = profile.Id;
            //newProfile.Name = profile.Name;
            //newProfile.Kind = profile.Kind;

            //var dialog = new ProfileWindow
            //{
            //    DataContext = new ProfileWindowViewModel(newProfile)
            //};
            //dialog.Owner = Application.Current.MainWindow;
            //WeakReferenceMessenger.Default.Register<CloseDialogMessage>(dialog, (r, m) =>
            //{
            //    dialog.DialogResult = m.Result;
            //});
            //try
            //{
            //    if (dialog.ShowDialog() ?? false)
            //    {
            //        await _profileService.UpdateProfileAsync(newProfile);

            //        profile.Name = newProfile.Name;
            //        profile.Kind = newProfile.Kind;
            //    }
            //}
            //finally
            //{
            //    WeakReferenceMessenger.Default.UnregisterAll(dialog);
            //}
        }

        [RelayCommand]
        private async Task RemoveProfile(ProfileModel profile)
        {
            //if (profile == null)
            //    return;

            //var dialog = new MessageWindow
            //{
            //    DataContext = new MessageWindowViewModel("Delete", $"Delete profile \"{profile.Name}\"?")
            //};
            //dialog.Owner = Application.Current.MainWindow;
            //dialog.ShowInTaskbar = false;
            //WeakReferenceMessenger.Default.Register<CloseDialogMessage>(dialog, (r, m) =>
            //{
            //    dialog.DialogResult = m.Result;
            //});
            //try
            //{
            //    if (dialog.ShowDialog() ?? false)
            //    {
            //        await _profileService.DeleteProfileAsync(profile.Id);
            //        _appService.Options.Profile = null;
            //        await _appService.SaveOptionsAsync();
                    
            //        Options.Profiles.Remove(profile);
            //        Options.CurrentProfile = null;
            //    }
            //}
            //finally
            //{
            //    WeakReferenceMessenger.Default.UnregisterAll(dialog);
            //}
        }

        [RelayCommand]
        private void ClearProfile(ProfileModel profile)
        {
            Options.CurrentProfile = null;
        }

        [RelayCommand]
        private void OpenScreenshotPath()
        {
            Microsoft.Win32.OpenFolderDialog dialog = new();
            dialog.Multiselect = false;
            dialog.Title = "Select a folder for screenshots";
            dialog.InitialDirectory = Options.SreenshotPath;
            
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                Options.SreenshotPath = dialog.FolderName;
            }
        }

#if DEBUG
        string[] _filenames =
        {
            @"2025-09-08[09-51]_-10.97, 1.40, -133.66_0.06718, -0.05115, 0.00348, 0.99642_15.00 (0).png",
            @"2025-09-07[15-31]_16.37, 1.29, -28.89_-0.00665, -0.99336, 0.07853, -0.08384_18.71 (0).png",
            @"2025-09-07[18-08]_415.53, 2.80, -71.77_0.00379, 0.08995, -0.00034, 0.99594_13.02 (0).png", // на новую заправку
            @"2025-09-07[18-08]_415.11, 2.80, -71.96_0.00297, 0.99312, 0.02581, -0.11414_13.04 (0).png", // в обратную сторону
            @"2025-09-07[18-08]_415.44, 2.80, -72.07_-0.01869, 0.73077, 0.02003, 0.68208_13.03 (0).png" // на ангар со снайпером
        };
        int _fileindex = -1;
#endif

        [RelayCommand]
        private void TestCurrentPosition()
        {
#if DEBUG             
            _fileindex++;
            if (_fileindex >= _filenames.Length)
            { 
                _fileindex = 0;
            }

            PositionModel? position = PositionModel.Parse(_filenames[_fileindex]);
            if (position != null)
            {
                InteractiveMap.CurrentPosition = position;
                InteractiveMap.CurrentPosition.IsVisibile = true;
                InteractiveMap.NormalizePosition(InteractiveMap.CurrentPosition);
            }
            else
            {
                InteractiveMap.CurrentPosition.IsVisibile = false;
            }
#endif
        }

        #endregion

        private async Task LoadMaps()
        {
            var maps = await _webApiService.GetMapsAsync();

            Maps.Clear();
            foreach (var entity in maps)
                Maps.Add(new MapModel(entity));
        }
     
        private void OnOptionsChanged(object? sender, EventArgs e)
        {
            Options.AppllyTo(_appService.Options);
            _ = _appService.SaveOptionsAsync();
        }

        void IDisposable.Dispose()
        {
            if (_fileMonitor is IDisposable monitor)
            {
                monitor.Dispose();
            }
        }
    }
}
