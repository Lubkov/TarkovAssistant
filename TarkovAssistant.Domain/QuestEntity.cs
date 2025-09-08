namespace TarkovAssistant.Domain
{    
    public class QuestEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public TraderKind Trader { get; set; } = TraderKind.None;

        public List<MarkerEntity> Markers { get; set; } = [];

        public QuestEntity()
        {            
        }

        public QuestEntity(string name, TraderKind trader)
        {
            Name = name;
            Trader = trader;
        }
    }
}
