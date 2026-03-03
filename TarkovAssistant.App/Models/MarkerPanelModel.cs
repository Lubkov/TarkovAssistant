using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using TarkovAssistant.Contracts;
using TarkovAssistant.Domain;

namespace TarkovAssistant.App.Models
{
    public partial class MarkerPanelModel : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        private string _description = string.Empty;

        public MarkerKind Kind { get; set; }

        [ObservableProperty]
        private string _traderPicture;

        public int? MapId { get; set; }

        public int? QuestId { get; set; }

        [ObservableProperty]
        private bool _isFinished = false;

        partial void OnIsFinishedChanged(bool value)
        {
            FinishedChanged?.Invoke(value);
        }

        [ObservableProperty]
        private MarkerResourceModel _currentScreenshot;

        [ObservableProperty]
        private Visibility _isScreenshotListVisible;

        [ObservableProperty]
        private Visibility _isVerticalItemListVisible;

        [ObservableProperty]
        private Visibility _isHorizontalItemListVisible;
        
        [ObservableProperty]
        private ObservableCollection<MarkerResourceModel> _screenshots = [];

        [ObservableProperty]
        private ObservableCollection<MarkerResourceModel> _items = [];

        public event Action<bool>? FinishedChanged;

        public MarkerPanelModel(MarkerFullDto marker)
        {
            ArgumentNullException.ThrowIfNull(marker);

            Id = marker.Id;
            Description = marker.Quest?.Name ?? string.Empty;
            Kind = marker.Kind;
            MapId = marker.MapId;
            QuestId = marker.Quest?.Id;
            TraderPicture = GetTraderPicture(marker?.Quest?.Trader ?? TraderKind.None);
            IsFinished = marker.IsFinished;

            foreach (var item in marker!.Resources)
            {
                switch (item.Kind)
                {
                    case ResourceKind.Quest:
                        Items.Add(new MarkerResourceModel(item));
                        break;

                    case ResourceKind.Screenshot:
                        Screenshots.Add(new MarkerResourceModel(item));
                        break;
                }
            }

            int heigh = 0, width = 0;
            foreach (var bmp in Items)
            {
                heigh += bmp.Picture?.PixelHeight ?? 0;
                width += bmp.Picture?.PixelWidth ?? 0;
            }

            CurrentScreenshot = Screenshots.FirstOrDefault();
            IsScreenshotListVisible = (Screenshots.Count > 1) ? Visibility.Visible : Visibility.Collapsed;

            if (Items.Count > 0)
            {
                if (width > heigh)
                {
                    IsVerticalItemListVisible = Visibility.Collapsed;
                    IsHorizontalItemListVisible = Visibility.Visible;
                }
                else
                {
                    IsVerticalItemListVisible = Visibility.Visible;
                    IsHorizontalItemListVisible = Visibility.Collapsed;
                }
            }
            else
            {
                IsVerticalItemListVisible = Visibility.Collapsed;
                IsHorizontalItemListVisible = Visibility.Collapsed;
            }
        }

        private string GetTraderPicture(TraderKind trader)
        {
            const string path = @"/Resources/Images/Traders/";

            switch (trader)
            {
                case TraderKind.Prapor:
                    return path + "trader_prapor.png";
                case TraderKind.Therapist:
                    return path + "trader_therapist.png";
                case TraderKind.Skier:
                    return path + "trader_skier.png";
                case TraderKind.Peacemaker:
                    return path + "trader_peacemaker.png";
                case TraderKind.Mechanic:
                    return path + "trader_mechanic.png";
                case TraderKind.Ragman:
                    return path + "trader_ragman.png";
                case TraderKind.Jaeger:
                    return path + "trader_jaeger.png";
                case TraderKind.Fence:
                    return path + "trader_fence.png";
                case TraderKind.Lightkeeper:
                    return path + "trader_lightkeeper.png";
                default:
                    return "trader_unknown.png";
            }
        }
    }
}
