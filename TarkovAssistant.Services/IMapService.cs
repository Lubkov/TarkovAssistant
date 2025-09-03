using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public interface IMapService
    {
        Task<GameMap?> GetMapByIdAsync(int mapId);
        Task<List<GameMap>> GetMapsAsync();
        Task<List<GameLayer>> GetLayersForMapAsync(int mapId);
        Task LoadLayersForMapAsync(GameMap map);
        Task<List<GameMarker>> GetMarkersForMapAsync(int mapId);
        Task<List<GameQuest>> GetQuestsForMapAsync(int mapId);
    }
}
