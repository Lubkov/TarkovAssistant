using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public interface IMapService
    {
        Task<MapEntity?> GetMapByIdAsync(int mapId, int? profileId);
        Task<MapEntity?> GetMapByIdAsync(int mapId);        
        Task<List<MapEntity>> GetMapsAsync();
        Task<List<LayerEntity>> GetLayersForMapAsync(int mapId);
        Task LoadLayersForMapAsync(MapEntity map);
        Task<List<MarkerEntity>> GetMarkersForMapAsync(int mapId, int? profileId);
        Task<List<MarkerEntity>> GetMarkersForMapAsync(int mapId);
        Task<List<QuestEntity>> GetQuestsForMapAsync(int mapId);
    }
}
