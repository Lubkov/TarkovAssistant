using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace TarkovAssistant.App.Models
{
    public partial class FormModel : ObservableObject
    {
        [ObservableProperty]
        private double _left;

        [ObservableProperty]
        private double _top;

        [ObservableProperty]
        private double _width;

        [ObservableProperty]
        private double _height;

        [ObservableProperty]
        private WindowStyle _windowStyle;

        [ObservableProperty]
        private ResizeMode _resizeMode;

        public FormModel() 
        {
            Left = 50;
            Top = 50;
            Width = 1020;
            Height = 740;
            WindowStyle = WindowStyle.SingleBorderWindow;
            ResizeMode = ResizeMode.CanResize;
        }

        public FormModel Clone()
        { 
            return (FormModel)this.MemberwiseClone();
        }
    }
}
