using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Data;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public class MarkerService : IMarkerService
    {
        private readonly ApplicationDbContext _db;

        public MarkerService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<GameMarker>> GetMarkersAsync()
        { 
            return await _db.Marker.ToListAsync();
        }
    }
}
