using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Data.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<ResourceEntity>
    {
        public void Configure(EntityTypeBuilder<ResourceEntity> builder)
        { 
            builder
                .ToTable("Resource")
                .HasKey(r => r.Id);

            builder
                .Property(r => r.Kind)
                .IsRequired();

            builder
                .Property(r => r.Description)
                .IsRequired();
        }
    }
}
