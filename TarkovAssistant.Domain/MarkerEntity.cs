namespace TarkovAssistant.Domain
{
    public class MarkerEntity
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        public MarkerKind Kind { get; set; }

        public int Left { get; set; }

        public int Top { get; set; }

        public MapEntity? Map { get; set; }

        public int? MapId { get; set; }

        public QuestEntity? Quest { get; set; }

        public int? QuestId { get; set; }

        public List<ResourceEntity> Resources { get; set; } = [];

        public List<PictureEntity> Pictures { get; set; } = [];

        public List<ProfileEntity> Profiles { get; set; } = [];

        public List<MarkerStateEntity> MarkerStates { get; set; } = [];
    }
}
