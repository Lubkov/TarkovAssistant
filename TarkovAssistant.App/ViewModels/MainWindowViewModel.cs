using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TarkovAssistant.App.Models;
using TarkovAssistant.Services;

namespace TarkovAssistant.App.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IMapService _mapService;

        private readonly FormWrapper _storedForm;

        public MapModel? CurrentMap { get; private set; }

        #region <Properties>

        [ObservableProperty]
        private FormModel _formInfo;               
                        
        [ObservableProperty]
        private bool _isFilterPanelVisible;

        [ObservableProperty]
        private bool _isMapSelectionPanelVisible;

        [ObservableProperty]
        private double _containerLeft;

        [ObservableProperty]
        private double _containerTop;

        [ObservableProperty]
        private double _containerWidth;

        [ObservableProperty]
        private double _containerHeight;

        [ObservableProperty]
        private MapModel? _selectedMap = null;

        partial void OnSelectedMapChanged(MapModel? value)
        {
            IsMapSelectionPanelVisible = false;
            if (value != null)
                _ = OpenSelectedMap(value);
        }

        [ObservableProperty]
        private LayerModel? _currentLayer = null;

        [ObservableProperty]
        private double _mapWidth;

        [ObservableProperty]
        private double _mapHeight;

        [ObservableProperty]
        private ObservableCollection<MapModel> _maps = new();

        [ObservableProperty]
        private ObservableCollection<MarkerModel> _markers = new();

        [ObservableProperty]
        private ObservableCollection<MarkerGroupModel> _quests = new();

        [ObservableProperty]
        private ObservableCollection<MarkerGroupModel> _extractions = new();
    
        #endregion

        public MainWindowViewModel(IMapService mapService)
        {
            _mapService = mapService;
            _formInfo = new FormModel();
            _storedForm = new FormWrapper(_formInfo);
        }

        #region <Commands>   
        
        [RelayCommand]
        private async Task SelectMap()
        {
            if (Maps.Count == 0)
            {
                await LoadMaps();
            }

            SelectedMap = null;
            IsMapSelectionPanelVisible = true;
        }

        private async Task OpenSelectedMap(MapModel map)
        {
            IsMapSelectionPanelVisible = false;
            if (CurrentMap == map)
            {
                return;
            }
            CurrentMap = map;
         
            await LoadLayers(CurrentMap);
            CurrentLayer = CurrentMap.MainLayer;
            MapWidth = CurrentLayer?.Picture?.Width ?? 0;
            MapHeight = CurrentLayer?.Picture?.Height ?? 0;
        }

        [RelayCommand]
        private void ToggleFullScreen()
        {
            _storedForm.IsFullScreen = !_storedForm.IsFullScreen;          
        }

        [RelayCommand]
        private void ZoomIn()
        {
            MapWidth += MapWidth * 0.2;
            MapHeight += MapHeight * 0.2;
        }

        [RelayCommand]
        private void ZoomOut()
        {
            MapWidth -= MapWidth * 0.2;
            MapHeight -= MapHeight * 0.2;
        }

        [RelayCommand]
        private void CenterMap()
        {
            ContainerLeft = (ContainerWidth - MapWidth) / 2;
            ContainerTop = (ContainerHeight - MapHeight) / 2;
        }

        [RelayCommand]
        private void ToggleMapFilters()
        {
            IsFilterPanelVisible = !IsFilterPanelVisible;
        }

        [RelayCommand]
        private void OpenSettings()
        {

        }

        #endregion

        private async Task LoadMaps()
        {            
            var maps = await _mapService.GetMapsAsync();

            Maps.Clear();
            foreach (var entity in maps)
                Maps.Add(new MapModel(entity));
        }

        private async Task LoadLayers(MapModel map)
        {
            var layers = await _mapService.GetLayersForMapAsync(map.Id);
            map.SetLayers(layers);
        }

        //private async Task OpenMap()
        //{
        //    CurrentMap = Maps.FirstOrDefault();
        //    if (CurrentMap != null)
        //    {
        //        MapWidth = Math.Abs(CurrentMap.Left) + Math.Abs(CurrentMap.Right);
        //        MapHeight = Math.Abs(CurrentMap.Right) + Math.Abs(CurrentMap.Bottom);

        //        await LoadLayers(CurrentMap);

        //        CurrentLayer = CurrentMap.MainLayer;
        //    }


        //    //CurrentMap = TestDataService.GenerateMap();
        //    //MapWidth = Math.Abs(CurrentMap.Left) + Math.Abs(CurrentMap.Right);
        //    //MapHeight = Math.Abs(CurrentMap.Right) + Math.Abs(CurrentMap.Bottom);

        //    //Quests = TestDataService.GenerateQuestRepository();
        //    //Extractions = TestDataService.GenerateExtractionRepository();
        //    //Markers.Clear();

        //    //foreach (var group in Extractions)
        //    //{
        //    //    foreach(var item in group.Markers)
        //    //    {
        //    //        Markers.Add(item);
        //    //    }                
        //    //}
        //}
    }
}
