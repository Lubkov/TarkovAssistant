using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public interface IMarkerStateService
    {
        Task<List<MarkerStateEntity>> GetMarkerStatesAsync();

        Task<MarkerStateEntity?> GetByKeysAsync(int profileId, int markerId);
        Task AddAsync(MarkerStateEntity state);
        Task AddAsync(int profileId, int markerId, bool isSeleced, bool isFinished);
        Task UpdateAsync(MarkerStateEntity state);
        Task UpdateAsync(int profileId, int markerId, bool isSeleced, bool isFinished);
        Task SaveAsync(int profileId, int markerId, bool isSeleced, bool isFinished);
        Task DeleteAsync(int profileId, int markerId);
    }
}
