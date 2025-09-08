namespace TarkovAssistant.Domain
{
    public class MapEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Left { get; set; }

        public int Top { get; set; }

        public int Right { get; set; }

        public int Bottom { get; set; }

        public byte[]? Picture { get; set; }

        public List<LayerEntity> Layers { get; set; } = [];

        public List<MarkerEntity> Markers { get; set; } = [];

        public MapEntity()
        { 
        
        }
    }
}
