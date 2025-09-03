using System.Globalization;
using System.Collections.ObjectModel;
using TarkovAssistant.Domain;
using TarkovAssistant.App.Localization;
using System.Runtime.ExceptionServices;

namespace TarkovAssistant.App.Models
{
    public class MarkerGroupModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public MarkerKind Kind { get; set; }

        private bool _selected;
        public bool IsSelected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    _selected = value;

                    if (Markers != null)
                    {
                        foreach (var item in Markers)
                        {
                            item.IsVisibile = _selected;
                        }
                    }                    

                    SelectedChanged?.Invoke(this);
                }
            }         
        }

        public event Action<MarkerGroupModel>? SelectedChanged;

        public string Icon { get; set; }

        public ObservableCollection<MarkerModel> Markers { get; private set; } = new();

        public MarkerGroupModel(MarkerKind kind)
        {            
            Name = MarkerModel.GetMarkerKindName(kind);
            Kind = kind;
            Icon = MarkerModel.GetMarkerIcon(kind);            
        }

        public void AddMarker(MarkerModel marker)
        {
            marker.IsVisibile = IsSelected;
            Markers.Add(marker);
        }

        public static MarkerGroupModel CreateFromQuest(GameQuest quest)
        {
            var group = new MarkerGroupModel(MarkerKind.Quest);
            group.Id = quest.Id;
            group.Name = quest.Name;
            //foreach (var marker in quest.Markers)
            //{
            //    group.Markers.Add(new MarkerModel(marker));
            //}

            return group;
        }
    }
}
