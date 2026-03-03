using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;
using TarkovAssistant.Contracts;
using TarkovAssistant.Domain;

namespace TarkovAssistant.App.Models
{
    public partial class MarkerResourceModel : ObservableObject
    {
        public ResourceKind Kind { get; set; }

        [ObservableProperty]
        private string _description;

        public BitmapImage? Picture { get; set; }

        public static string DataPath { get; set; } = string.Empty;

        public MarkerResourceModel(ResourceDto resource)
        {
            Kind = resource.Kind;
            Description = resource.Description ?? string.Empty;
            Picture = ImageHelper.GetPicture(resource.Picture);
        }
    }
}
