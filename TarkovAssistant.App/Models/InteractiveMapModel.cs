using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TarkovAssistant.Domain;
using TarkovAssistant.Services;

namespace TarkovAssistant.App.Models
{
    public partial class InteractiveMapModel : ObservableObject
    {
        private IAppService AppService { get; set; }
        private IMapService MapService { get; set; }
        private IMarkerService MarkerService { get; set; }
        private IMarkerStateService MarkerStateService { get; set; }
        private IFileMonitor FileMonitor { get; set; }

        #region <Properties>

        [ObservableProperty]
        private MapModel? _map;

        [ObservableProperty]
        private double _width;

        [ObservableProperty]
        private double _height;

        [ObservableProperty]
        private double _containerLeft;

        [ObservableProperty]
        private double _containerTop;

        [ObservableProperty]
        private double _containerWidth;

        [ObservableProperty]
        private double _containerHeight;

        //[ObservableProperty]
        //private ObservableCollection<LayerModel> _layers = new();

        [ObservableProperty]
        private LayerModel? _currentLayer = null;

        [ObservableProperty]
        private PositionModel _currentPosition = new();

        [ObservableProperty]
        private MarkerPanelModel? _currentMarker;

        partial void OnCurrentLayerChanged(LayerModel? value)
        {
            Width = CurrentLayer?.Picture?.Width ?? 0;
            Height = CurrentLayer?.Picture?.Height ?? 0;
        }

        [ObservableProperty]
        private ObservableCollection<MarkerModel> _markers = [];

        [ObservableProperty]
        private ObservableCollection<MarkerGroupModel> _extractions = [];

        [ObservableProperty]
        private ObservableCollection<MarkerGroupModel> _quests = [];

        private Dictionary<MarkerKind, MarkerGroupModel> MarkerGroups { get; set; }

        #endregion

        public InteractiveMapModel(
            IAppService appService,
            IMapService mapService,
            IMarkerService markerService,
            IMarkerStateService markerStateService,
            IFileMonitor fileMonitor)
        {
            AppService = appService;
            MapService = mapService;
            MarkerService = markerService;
            MarkerStateService = markerStateService;
            FileMonitor = fileMonitor;

            MarkerGroups = new Dictionary<MarkerKind, MarkerGroupModel>();
            foreach (MarkerKind kind in Enum.GetValues(typeof(MarkerKind)))
            {
                MarkerGroups.Add(kind, new MarkerGroupModel(kind));
            }

            Extractions.Add(MarkerGroups[MarkerKind.PMCExtraction]);
            Extractions.Add(MarkerGroups[MarkerKind.ScavExtraction]);
            Extractions.Add(MarkerGroups[MarkerKind.CoopExtraction]);
            Extractions.Add(MarkerGroups[MarkerKind.TransitExtraction]);

            FileMonitor.FileCreated += OnFileCreated;
            FileMonitor.Start(AppService.Options.SreenshotPath, "*.png");

            MarkerGroups[MarkerKind.Quest].IsSelected = true;
        }

        public async Task Open(MapModel map)
        {
            if (this.Map?.Id == map.Id)
            {
                return;
            }

            CurrentPosition.IsVisibile = false;
            await LoadMap(map.Id);
        }

        public async Task<bool> OpenMarker(int markerId)
        {
            if (CurrentMarker?.Id == markerId)
            {
                return true;
            }

            var marker = await MarkerService.GetMarkerById(markerId, AppService.Options.Profile);
            if (marker == null)
            {
                return false;
            }

            CurrentMarker = new MarkerPanelModel(marker);
            return true;
        }

        private async Task LoadMap(int mapId)
        {
            var map = await MapService.GetMapByIdAsync(mapId, AppService.Options.Profile);

            CurrentLayer = null;
            Markers.Clear();
            Quests.Clear();
            foreach (var group in MarkerGroups.Values)
            {
                group.Markers.Clear();
            }

            if (map != null)
            {
                Map = new MapModel(map);
                Map.SetLayers(map.Layers);
                CurrentLayer = Map.MainLayer;
                SetMarkers(map);
            }
            else
            {
                Map = null;
            }
        }

        private void SetMarkers(MapEntity map)
        {
            Debug.WriteLine("Set markers");

            HashSet<QuestEntity> quests = new HashSet<QuestEntity>();

            foreach (var marker in map.Markers)
            {
                if (marker.Kind != MarkerKind.Quest)
                {
                    var item = new MarkerModel(marker);
                    NormalizePosition(item);
                    Markers.Add(item);
                    MarkerGroups[marker.Kind].AddMarker(item);
                }
                else
                {
                    quests.Add(marker.Quest!);
                }
            }

            foreach (var quest in quests.OrderBy(q => q.Name).ToList())
            {
                SetQuestMarkers(map.Id, quest);
            }
        }

        private void SetQuestMarkers(int mapId, QuestEntity quest)
        {
            var group = MarkerGroupModel.CreateFromQuest(quest);
            var selected = false;
            var finished = true;
            foreach (var marker in quest.Markers)
            {
                if (marker.MapId != mapId)
                    continue;

                var item = new MarkerModel(marker);
                NormalizePosition(item);
                Markers.Add(item);
                MarkerGroups[marker.Kind].AddMarker(item);

                item.StateChanged += async (object? sender, EventArgs e) =>
                {
                    if (sender is MarkerModel marker)
                    {
                        if (AppService.Options.Profile != null)
                        {
                            await MarkerStateService.SaveAsync(
                                AppService.Options.Profile.Value,
                                marker.Id,
                                marker.IsVisibile,
                                marker.IsFinished);

                            group.UpdatePressed();
                        }
                    }
                };

                group.Markers.Add(item);
                selected = selected || item.IsVisibile;
                finished = finished && item.IsFinished;
            }

            group.IsSelected = selected;
            group.IsPressed = finished && group.Markers.Count > 0;

            Quests.Add(group);
        }

        public void ZoomIn()
        {
            Width += Width * 0.2;
            Height += Height * 0.2;

            NormalizeAllMarkerPositions();
        }

        public void ZoomOut()
        {
            Width -= Width * 0.2;
            Height -= Height * 0.2;

            NormalizeAllMarkerPositions();
        }

        public void CenterMap(FormModel formInfo)
        {
            var height = (ContainerHeight < 1) ? formInfo.Height : ContainerHeight;
            var width = (ContainerWidth < 1) ? formInfo.Width : ContainerWidth;
            ContainerTop = (height - Height) / 2;
            ContainerLeft = (width - Width) / 2;
        }

        public void NormalizePosition(PositionModel point)
        {
            const int iconHeight = 32;
            const int iconWidth = 32;
            const int LaboratoryId = 8;

            if (Map != null)
            {
                if (Map.Id == LaboratoryId)
                {
                    double offset = Math.Abs((Map.Top - point.OriginTop) / (Map.Bottom - Map.Top));
                    point.Left = Width * offset - iconWidth / 2;
                    offset = Math.Abs((Map.Left - point.OriginLeft) / (Map.Right - Map.Left));
                    point.Top = Height * offset - iconHeight / 2;
                }
                else
                {
                    double offset = Math.Abs((Map.Top - point.OriginTop) / (Map.Bottom - Map.Top));
                    point.Top = Height * offset - iconHeight / 2;
                    offset = Math.Abs((Map.Left - point.OriginLeft) / (Map.Right - Map.Left));
                    point.Left = Width * offset - iconWidth / 2;
                }
            }
            else
            {
                point.Top = point.OriginTop;
                point.Left = point.OriginLeft;
            }
        }

        private void NormalizeAllMarkerPositions()
        {
            foreach (var marker in Markers)
            {
                NormalizePosition(marker);
            }

            NormalizePosition(CurrentPosition);
        }

        private void OnFileCreated(object? sender, FileCreatedEventArgs e)
        {
            PositionModel? position = PositionModel.Parse(e.FileName);
            if (position != null)
            {
                CurrentPosition = position;
                CurrentPosition.IsVisibile = true;
                NormalizePosition(CurrentPosition);
            }
            else
            {
                CurrentPosition.IsVisibile = false;
            }
        }
    }
}
