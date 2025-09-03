using System.IO;
using System.Windows.Media.Imaging;
using TarkovAssistant.Domain;

namespace TarkovAssistant.App.Models
{
    public class LayerModel
    {
        public int Id { get; set; }

        public LayerLevel Level { get; set; }

        public string? Name { get; set; }
        
        public bool IsMainLayer { get; set; }

        public BitmapImage? Picture { get; set; }

        public LayerModel(GameLayer layer)
        { 
            Id = layer.Id;
            Level = layer.Level;
            Name = layer.Name;
            IsMainLayer = layer.IsMainLayer();
            Picture = ImageHelper.GetPicture(layer.Picture);
        }       
    }
}
