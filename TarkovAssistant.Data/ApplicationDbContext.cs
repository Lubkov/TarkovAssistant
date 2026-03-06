using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TarkovAssistant.Data.Configurations;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserEntity>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<MapEntity> Maps { get; set; } = null!;
        public DbSet<LayerEntity> Layers { get; set; } = null!;
        public DbSet<MarkerEntity> Markers { get; set; } = null!;
        public DbSet<QuestEntity> Quests { get; set; } = null!;
        public DbSet<ResourceEntity> Resources { get; set; } = null!;
        public DbSet<PictureEntity> Pictures { get; set; } = null!;
        public DbSet<ProfileEntity> Profiles { get; set; } = null!;
        public DbSet<MarkerStateEntity> MarkerStates { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MapConfiguration());
            modelBuilder.ApplyConfiguration(new LayerConfiguration());
            modelBuilder.ApplyConfiguration(new QuestConfiguration());
            modelBuilder.ApplyConfiguration(new MarkerConfiguration());
            modelBuilder.ApplyConfiguration(new ResourceConfiguration());
            modelBuilder.ApplyConfiguration(new PictureConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileConfiguration());
            modelBuilder.ApplyConfiguration(new MarkerStateConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
