using TarkovAssistant.Domain;

namespace TarkovAssistant.Contracts
{
    public class MarkerDto
    {
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public MarkerKind Kind { get; set; }

        public int Left { get; set; }

        public int Top { get; set; }

        public int? MapId { get; set; }

        public int? QuestId { get; set; }

        public MarkerDto(MarkerEntity source)
        { 
            Id = source.Id;
            Description = source.Description ?? string.Empty;
            Kind = source.Kind;
            Left = source.Left;
            Top = source.Top;
            MapId = source.MapId;
            QuestId = source.QuestId;
        }
    }
}
