using TarkovAssistant.Domain;

namespace TarkovAssistant.Contracts
{
    public class MapDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Picture { get; set; } = string.Empty;

        public MapDto()
        {
        }

        public MapDto(MapEntity source)
        {
            Id = source.Id;
            Name = source.Name;
            Picture = source.Resource?.Hash ?? "";
        }
    }
}
