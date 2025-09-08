using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;
using TarkovAssistant.Domain;

namespace TarkovAssistant.App.Models
{
    public partial class MarkerResourceModel : ObservableObject
    {
        public int Id { get; set; }
        public ResourceKind Kind { get; set; }

        [ObservableProperty]
        private string _description;

        public BitmapImage? Picture { get; set; }

        public static string DataPath { get; set; } = string.Empty;

        public MarkerResourceModel(ResourceEntity resource)
        { 
            Id = resource.Id;
            Kind = resource.Kind;
            Description = resource.Description ?? string.Empty;

            var filename = Path.Combine(DataPath, GetResourceFileName(Id, Kind));
            Picture = ImageHelper.GetPicture(filename);
        }

        public static string GetResourceFileName(int key, ResourceKind kind)
        {
            switch (kind)
            {
                case ResourceKind.Screenshot:
                    return Path.Combine("markers", $"Resource_{key}.jpg");
                case ResourceKind.Quest:
                    return Path.Combine("items", $"Resource_{key}.png");
                default:
                    return string.Empty;
            }
        }
    }
}
