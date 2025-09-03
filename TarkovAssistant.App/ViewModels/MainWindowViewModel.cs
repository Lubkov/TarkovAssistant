using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TarkovAssistant.App.Models;
using TarkovAssistant.Services;

namespace TarkovAssistant.App.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject, IDisposable
    {
        private readonly IMapService _mapService;
        private readonly IFileMonitor _fileMonitor;

        private readonly FormWrapper _storedForm;

        #region <Properties>

        [ObservableProperty]
        private FormModel _formInfo;

        [ObservableProperty]
        private InteractiveMapModel _interactiveMap;                
                        
        [ObservableProperty]
        private bool _isFilterPanelVisible;

        [ObservableProperty]
        private bool _isMapSelectionPanelVisible;

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

        public MainWindowViewModel(IMapService mapService, IFileMonitor fileMonitor)
        {
            _mapService = mapService;
            _fileMonitor = fileMonitor;
            _formInfo = new FormModel();
            _storedForm = new FormWrapper(_formInfo);
            _interactiveMap = new InteractiveMapModel(mapService, fileMonitor);
#if DEBUG
            _isTestMode = true;
#else
            _isTestMode = false;
#endif
        }

        #region <Commands>   

        [RelayCommand]
        private async Task SelectMap()
        {
            if (Maps.Count == 0)
            {
                await LoadMaps();
            }

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
            IsFilterPanelVisible = !IsFilterPanelVisible;
        }

        [RelayCommand]
        private void OpenSettings()
        {
            HideAllPanels();
        }

        [RelayCommand]
        private void HideAllPanels()
        {
            IsMapSelectionPanelVisible = false;
            IsFilterPanelVisible = false;
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
            var maps = await _mapService.GetMapsAsync();

            Maps.Clear();
            foreach (var entity in maps)
                Maps.Add(new MapModel(entity));
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
