using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using TarkovAssistant.Domain;

namespace TarkovAssistant.App.Models
{
    public partial class MarkerGroupModel : ObservableObject
    {
        public int Id { get; set; }

        public bool IsSubGroup { get; set; }

        public string Name { get; set; }

        public MarkerKind Kind { get; set; }

        [ObservableProperty]
        private bool _isSelected;

        partial void OnIsSelectedChanged(bool value)
        {
            foreach (var item in Markers)
            {
                item.IsVisibile = value;
            }

            UpdateFontColors();
        }

        [ObservableProperty]
        private bool _isPressed;

        partial void OnIsPressedChanged(bool value)
        {
            UpdateFontColors();
        }

        public string Icon { get; set; }

        public ObservableCollection<MarkerModel> Markers { get; private set; } = [];

        [ObservableProperty]
        private SolidColorBrush _fontColor = null!;

        [ObservableProperty]
        private SolidColorBrush _pressedFontColor = null!;

        public MarkerGroupModel(MarkerKind kind)
        {            
            Name = MarkerModel.GetMarkerKindName(kind);
            Kind = kind;
            Icon = MarkerModel.GetMarkerIcon(kind, false);
            IsSubGroup = false;
            IsSelected = false;
            IsPressed = false;

            UpdateFontColors();
        }

        public void AddMarker(MarkerModel marker)
        {
            if (Kind != MarkerKind.Quest)
            {
                marker.IsVisibile = IsSelected;
            }

            Markers.Add(marker);
        }

        public static MarkerGroupModel CreateFromQuest(QuestEntity quest)
        {
            var group = new MarkerGroupModel(MarkerKind.Quest);
            group.Id = quest.Id;
            group.IsSubGroup = true;
            group.Name = quest.Name;

            return group;
        }

        public void UpdatePressed()
        {
            IsPressed = Markers.Count > 0 && Markers.Where(m => m.IsFinished == false).ToList().Count == 0;
        }

        private SolidColorBrush GetFontColor()
        {
            if (IsPressed)
            {
                return (SolidColorBrush)App.Current.TryFindResource("MediumFocudedBrush");
            }
            else
            {
                return (SolidColorBrush)App.Current.TryFindResource("LightBrush");
            }
        }

        private SolidColorBrush GetPressedFontColor()
        {
            if (IsPressed)
            {
                return (SolidColorBrush)App.Current.TryFindResource("BaseFocudedBrush");
            }
            else
            {
                return (SolidColorBrush)App.Current.TryFindResource("FilterSelectedBrush");
            }
        }

        private void UpdateFontColors()
        {
            FontColor = GetFontColor();
            PressedFontColor = GetPressedFontColor();
        }
    }
}
