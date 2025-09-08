using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TarkovAssistant.Domain;

namespace TarkovAssistant.Data.Configurations
{
    public class MapConfiguration : IEntityTypeConfiguration<MapEntity>
    {
        public void Configure(EntityTypeBuilder<MapEntity> builder)
        {
            builder
                .ToTable("Map")
                .HasKey(map => map.Id);

            builder.Property(map => map.Name)
                .IsRequired()
                .HasMaxLength(64);

            builder
                .HasMany(map => map.Layers)
                .WithOne(layer => layer.Map);

            builder
                .HasMany(map => map.Markers)
                .WithOne(marker => marker.Map);
        }
    }
}
