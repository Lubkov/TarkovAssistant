using TarkovAssistant.Domain;

namespace TarkovAssistant.Contracts
{
    public class MapFullDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Left { get; set; }

        public int Top { get; set; }

        public int Right { get; set; }

        public int Bottom { get; set; }

        public List<LayerDto> Layers { get; set; } = [];

        public List<MarkerDto> Markers { get; set; } = [];

        public MapFullDto(MapEntity source) 
        { 
            Id = source.Id;
            Name = source.Name;
            Left = source.Left;
            Top = source.Top;
            Right = source.Right;
            Bottom = source.Bottom;

            Layers = source.Layers.Select(l => new LayerDto(l)).ToList();
            Markers = source.Markers.Select(m => new MarkerDto(m)).ToList();
        }
    }
}
