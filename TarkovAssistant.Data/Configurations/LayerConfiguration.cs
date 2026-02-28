using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Data.Configurations
{
    public class LayerConfiguration : IEntityTypeConfiguration<LayerEntity>
    {
        public void Configure(EntityTypeBuilder<LayerEntity> builder)
        {
            builder
                .ToTable("Layer")
                .HasKey(layer => layer.Id);

            builder
                .Property(layer => layer.Level)
                .IsRequired();

            builder
                .Property(layer => layer.Name)
                .IsRequired()
                .HasMaxLength(64);

            builder
                .HasOne(layer => layer.Map)
                .WithMany(map => map.Layers)
                .HasForeignKey(layer => layer.MapId);

            builder
                .HasOne(layer => layer.Resource)
                .WithOne(res => res.Layer);
        }
    }
}
