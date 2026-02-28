namespace TarkovAssistant.Domain
{
    public class ResourceEntity
    {
        public int Id { get; set; }

        public ResourceKind Kind { get; set; }

        public string? Description { get; set; }

        public string? Hash { get; set; }

        public List<MarkerEntity> Markers { get; set; } = [];

        public List<PictureEntity> Pictures { get; set; } = [];

        public MapEntity? Map { get; set; }

        public LayerEntity? Layer { get; set; }
    }
}
