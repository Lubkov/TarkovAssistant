namespace TarkovAssistant.Domain
{
    public class MarkerStateEntity
    {
        public int ProfileId { get; set; }
        public ProfileEntity? Profile { get; set; }
        public int MarkerId { get; set; }
        public MarkerEntity? Marker { get; set; }        
        public bool IsFinished { get; set; }
        public bool IsSeleced { get; set; }
    }
}
