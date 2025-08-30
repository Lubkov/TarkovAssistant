using System.Collections.ObjectModel;

namespace TarkovAssistant.App.Models
{
    public class MarkerGroupModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

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
                            item.Visibility = _selected ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                        }
                    }                    

                    SelectedChanged?.Invoke(this);
                }
            }         
        }

        public event Action<MarkerGroupModel>? SelectedChanged;

        public string? Icon { get; set; }

        public ObservableCollection<MarkerModel>? Markers { get; set; }

        public MarkerGroupModel() : this(string.Empty)
        {
        }

        public MarkerGroupModel(string name)
        {
            Name = name;
            IsSelected = false;            
        }

        public override string ToString()
        {
            return $"Group: {Name} ({Markers?.Count})";
        }
    }
}
