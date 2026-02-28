using TarkovAssistant.Contracts;

namespace TarkovAssistant.Services
{
    public interface IWebApiService
    {
        Task<List<MapSummaryDto>> GetMapsFromWebApiAsync();
        Task<MapFullDto?> GetMapByIdFromWebApiAsync(int mapId, int? profileId);
    }
}
