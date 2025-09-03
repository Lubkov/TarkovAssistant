using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public interface IMarkerService
    {
        Task<List<GameMarker>> GetMarkersAsync();
    }
}
