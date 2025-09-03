using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Data;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public class MapService : IMapService
    {
        private readonly ApplicationDbContext context;

        public MapService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<GameMap?> GetMapByIdAsync(int mapId)
        {
            return await context.Maps
                .Include(map => map.Layers)
                .Include(map => map.Markers)
                .ThenInclude(marker => marker!.Quest)
                .Where(map => map.Id == mapId).FirstOrDefaultAsync();
        }

        public async Task<List<GameMap>> GetMapsAsync()
        {
            return await context.Maps.ToListAsync();
        }

        //public async Task AddMapAsync(string name)
        //{
        //    context.Maps.Add(new GameMap { Name = name });
        //    await context.SaveChangesAsync();
        //}

        public async Task<List<GameLayer>> GetLayersForMapAsync(int mapId)
        {
            return await context.Layers
                .Where(layer => layer.MapId == mapId)
                .ToListAsync();
        }

        public async Task LoadLayersForMapAsync(GameMap map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            await context.Entry(map).Collection(m => m.Layers).LoadAsync();
        }

        public async Task<List<GameMarker>> GetMarkersForMapAsync(int mapId)
        {
            return await context.Marker.Where(marker => marker.MapId == mapId).ToListAsync();
        }

        public async Task<List<GameQuest>> GetQuestsForMapAsync(int mapId)
        {            
            return await context.Quests
                //.Include(q => q.Markers)
                .Where(q => q.Markers.Any(m => m.MapId == mapId))
                .ToListAsync();            
        }
    }
}
