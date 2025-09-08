using CommunityToolkit.Mvvm.ComponentModel;
using TarkovAssistant.Domain;

namespace TarkovAssistant.App.Models
{
    public partial class QuestModel : ObservableObject
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public TraderKind Trader { get; set; }
        public List<MarkerModel> Markers { get; set; } = new();

        public QuestModel(QuestEntity quest)
        { 
            Id = quest.Id;
            Name = quest.Name;
            Trader = quest.Trader;

            Markers.Clear();
            //Markers.AddRange(quest.Markers);
        }
    }
}
