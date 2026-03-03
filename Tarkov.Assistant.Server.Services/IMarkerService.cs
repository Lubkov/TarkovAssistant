using TarkovAssistant.Domain;

namespace TarkovAssistant.Server.Services
{
    public interface IMarkerService
    {
        Task<List<MarkerEntity>> GetMarkersAsync();
        Task<MarkerEntity?> GetMarkerByIdAsync(int id, int? profileId);
    }
}
