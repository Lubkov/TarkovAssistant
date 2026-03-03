using TarkovAssistant.Contracts;

namespace TarkovAssistant.Services
{
    public interface IWebApiService
    {
        Task<List<MapDto>> GetMapsAsync();
        Task<MapFullDto?> GetMapByIdAsync(int mapId, int? profileId);
        Task<MarkerFullDto?> GetMarkerByIdAsync(int id, int? profileId);
        Task<bool> SaveMarkerStateAsync(int profileId, int markerId, bool isSeleced, bool isFinished);
        Task<List<ProfileDto>> GetProfilesAsync();
    }
}