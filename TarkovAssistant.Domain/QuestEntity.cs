namespace TarkovAssistant.Domain
{    
    public class QuestEntity : IEquatable<QuestEntity>
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public TraderKind Trader { get; set; } = TraderKind.None;

        public List<MarkerEntity> Markers { get; set; } = [];

        public QuestEntity()
        {
            Id = 0;
        }

        public QuestEntity(string name, TraderKind trader)
        {
            Name = name;
            Trader = trader;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as QuestEntity);
        }

        public bool Equals(QuestEntity? other)
        {
            return (other != null) && (other is QuestEntity q) && (Id == q.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
