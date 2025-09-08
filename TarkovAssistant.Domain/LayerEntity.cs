namespace TarkovAssistant.Domain
{
    public class LayerEntity
    {
        public int Id { get; set; }

        public LayerLevel Level { get; set; }

        public string? Name { get; set; }

        public byte[]? Picture { get; set; }

        public MapEntity? Map { get; set; }

        public int? MapId { get; set; }

        public bool IsMainLayer()
        {   
            return Level == LayerLevel.Main;
        }
    }
}
