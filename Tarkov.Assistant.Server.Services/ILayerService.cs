using TarkovAssistant.Domain;

namespace TarkovAssistant.Server.Services
{
    public interface ILayerService
    {
        Task<List<LayerEntity>> GetLayersAsync();
    }
}
