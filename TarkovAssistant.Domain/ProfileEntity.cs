namespace TarkovAssistant.Domain
{
    public class ProfileEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ProfileKind Kind { get; set; }

        public List<MarkerEntity> Markers { get; set; } = [];
        public List<MarkerStateEntity> MarkerStates { get; set; } = [];
    }
}
