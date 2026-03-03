using TarkovAssistant.Domain;

namespace TarkovAssistant.Contracts
{
    public class LayerDto
    {
        public int Id { get; set; }

        public LayerLevel Level { get; set; }

        public string? Name { get; set; }

        public int? MapId { get; set; }
        
        public string Picture { get; set; } = string.Empty;

        public bool IsMainLayer { get; set; } = false;

        public LayerDto()
        {
        }

        public LayerDto(LayerEntity source) 
        { 
            Id = source.Id;
            Level = source.Level;
            Name = source.Name;
            MapId = source.MapId;
            Picture = source.Resource?.Hash ?? "";
            IsMainLayer = source.IsMainLayer();
        }
    }
}
