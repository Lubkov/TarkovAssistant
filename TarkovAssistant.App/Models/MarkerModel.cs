using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace TarkovAssistant.App.Models
{
    public partial class MarkerModel : ObservableObject
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public double Left { get; set; }

        public double Top { get; set; }

        public string? Icon { get; set; }

        [ObservableProperty]
        private Visibility _visibility;

        public MarkerModel()
        { 
            Id = 0;
            Name = "";
            Left = 0;
            Top = 0;
            Visibility = Visibility.Hidden;
        }
    }
}
