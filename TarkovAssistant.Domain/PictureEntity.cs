namespace TarkovAssistant.Domain
{
    public class PictureEntity
    {
        public int MarkerId { get; set; }
        public MarkerEntity? Marker { get; set; }

        public int ResourceId { get; set; }
        public ResourceEntity? Resource { get; set; }

        public int Amount { get; set; }
    }
}
