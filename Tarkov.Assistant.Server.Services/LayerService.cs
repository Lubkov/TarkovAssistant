using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Data;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Server.Services
{
    public class LayerService : ILayerService
    {
        private readonly ApplicationDbContext _db;

        public LayerService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<LayerEntity>> GetLayersAsync()
        {
            return await _db.Layers.ToListAsync();
        }
    }
}
