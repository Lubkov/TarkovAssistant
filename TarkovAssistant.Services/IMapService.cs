using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public interface IMapService
    {
        Task<List<GameMap>> GetMapsAsync();
        Task<List<GameLayer>> GetLayersForMapAsync(int mapId);
        Task LoadLayersForMapAsync(GameMap map);

    }
}
