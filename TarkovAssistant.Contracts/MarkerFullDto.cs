using TarkovAssistant.Domain;

namespace TarkovAssistant.Contracts
{
    public class MarkerFullDto
    {
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public MarkerKind Kind { get; set; }

        public int Left { get; set; }

        public int Top { get; set; }

        public int? MapId { get; set; }

        public QuestDto? Quest { get; set; }

        public bool IsFinished { get; set; }

        public bool IsSeleced { get; set; }

        public List<ResourceDto> Resources { get; set; } = [];

        public MarkerFullDto()
        {
            Id = 0;
            Description = string.Empty;
            Kind = MarkerKind.PMCExtraction;
            Left = 0;
            Top = 0;
            MapId = 0;
            Quest = null;
            IsFinished = false;
            IsSeleced = false;
        }

        public MarkerFullDto(MarkerEntity source)
        {
            Id = source.Id;
            Description = source.Description ?? string.Empty;
            Kind = source.Kind;
            Left = source.Left;
            Top = source.Top;
            MapId = source.MapId;

            if (source.Quest != null)
            {
                Quest = new QuestDto(source.Quest);
            }

            IsFinished = source.MarkerStates.FirstOrDefault()?.IsFinished ?? false;
            IsSeleced = source.MarkerStates.FirstOrDefault()?.IsSeleced ?? false;
            Resources = source.Resources.Select(r => new ResourceDto(r)).ToList();
        }
    }
}
