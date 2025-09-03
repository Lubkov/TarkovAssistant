using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Data;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public class MapService : IMapService
    {
        private readonly ApplicationDbContext _db;

        public MapService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<GameMap>> GetMapsAsync()
        {
            return await _db.Maps.ToListAsync();
        }

        //public async Task AddMapAsync(string name)
        //{
        //    _db.Maps.Add(new GameMap { Name = name });
        //    await _db.SaveChangesAsync();
        //}

        public async Task<List<GameLayer>> GetLayersForMapAsync(int mapId)
        {
            return await _db.Layers
                .Where(layer => layer.MapId == mapId)
                .ToListAsync();
        }

        public async Task LoadLayersForMapAsync(GameMap map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            await _db.Entry(map).Collection(m => m.Layers).LoadAsync();
        }
    }
}
