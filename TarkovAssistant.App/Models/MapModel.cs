using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media.Imaging;
using TarkovAssistant.Contracts;

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

        public List<LayerModel> Layers { get; private set; } = [];

        public LayerModel? MainLayer { get; private set; }

        public MapModel() 
        {
            Name = "";
            Picture = new BitmapImage();
        }

        public MapModel(MapFullDto map)
        { 
            Id = map.Id;
            Name = map.Name;
            Left = map.Left;
            Top = map.Top;
            Right = map.Right;
            Bottom = map.Bottom;
        }

        public MapModel(MapDto map)
        {
            Id = map.Id;
            Name = map.Name;
            Picture = ImageHelper.GetPicture(map.Picture);            
        }

        public void SetLayers(List<LayerDto> layers)
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
    }
}
