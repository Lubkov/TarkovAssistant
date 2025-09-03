using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<GameMap> Maps { get; set; } = null!;
        public DbSet<GameLayer> Layers { get; set; } = null!;
        public DbSet<GameMarker> Marker { get; set; } = null!;
        public DbSet<GameQuest> Quests { get; set; } = null!;
   
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
