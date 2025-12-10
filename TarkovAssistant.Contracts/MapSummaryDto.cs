using TarkovAssistant.Domain;

namespace TarkovAssistant.Contracts
{
    public class MapSummaryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public MapSummaryDto(MapEntity source)
        {
            Id = source.Id;
            Name = source.Name;
        }
    }
}
