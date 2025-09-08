using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public interface IMarkerService
    {
        Task<List<MarkerEntity>> GetMarkersAsync();
        Task<MarkerEntity?> GetMarkerById(int id, int? profileId);
    }
}
