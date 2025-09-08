namespace TarkovAssistant.Domain
{
    public enum ProfileKind : int
    {
        Bear = 1, 
        Usec = 2
    }

    public class MarkerState
    { 
        public int MarkerId { get; set; }
        public bool IsFinished { get; set; }
        public bool IsSeleced { get; set; }        
    }

    public class Profile
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ProfileKind Kind { get; set; }
        public List<MarkerState> Markers { get; set; } = [];
    }   
}
