using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public interface ILayerService
    {
        Task<List<LayerEntity>> GetLayersAsync();        
    }
}
