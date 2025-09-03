using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using TarkovAssistant.Domain;

namespace TarkovAssistant.App.Models
{
    public partial class MapModel : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public BitmapImage? Picture { get; set; }

        public List<LayerModel> Layers { get; private set; } = new();

        public LayerModel? MainLayer { get; private set; }

        public List<MarkerModel> Markers { get; private set; } = new();

        public List<QuestModel> Quests { get; private set; } = new();

        public List<MarkerModel> this[MarkerKind kind] 
        {
            get 
            { 
                return Markers.Where(marker => marker.Kind == kind).ToList();
            }
        }

        public MapModel() 
        {
            Name = "";
            Picture = new BitmapImage();
        }

        public MapModel(GameMap map)
        { 
            Id = map.Id;
            Name = map.Name;
            Left = map.Left;
            Top = map.Top;
            Right = map.Right;
            Bottom = map.Bottom;
            Picture = ImageHelper.GetPicture(map.Picture);           

            SetLayers(map.Layers);            
        }

        public void SetLayers(List<GameLayer> layers)
        {
            Layers.Clear();

            if (layers == null || layers.Count == 0)
            {
                MainLayer = null;
                return;
            }

            Layers.AddRange(layers.Select(gl => new LayerModel(gl)));
            MainLayer = Layers.FirstOrDefault(layer => layer.IsMainLayer);
        }

        public void SetMarkers(List<GameMarker> source)
        {
            Markers.Clear();
            if (source == null || source.Count == 0)
            { 
                return; 
            }                       

            Markers.AddRange(source.Select(marker => new MarkerModel(marker)));
        }

        public void SetQuests(List<GameQuest> source)
        {
            Quests.Clear();
            if (source == null || source.Count == 0)
            {
                return;
            }

            Quests.AddRange(source.Select(quest => new QuestModel(quest)));
        }
    }
}
