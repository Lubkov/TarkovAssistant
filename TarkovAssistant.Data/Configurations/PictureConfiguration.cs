using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Data.Configurations
{
    public class PictureConfiguration : IEntityTypeConfiguration<PictureEntity>
    {
        public void Configure(EntityTypeBuilder<PictureEntity> builder)
        {
            builder.ToTable("Picture");
            builder.HasKey(p => new { p.MarkerId, p.ResourceId });
            
            builder.Property(p => p.Amount)
               .HasDefaultValue(1);

            builder
                .HasOne(picture => picture.Marker)
                .WithMany(marker => marker.Pictures)
                .HasForeignKey(picture => picture.MarkerId);

            builder
                .HasOne(picture => picture.Resource)
                .WithMany(res => res.Pictures)
                .HasForeignKey(picture => picture.ResourceId);
        }
    }
}
