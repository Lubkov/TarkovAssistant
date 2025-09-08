using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Data.Configurations
{
    public class MarkerConfiguration : IEntityTypeConfiguration<MarkerEntity>
    {
        public void Configure(EntityTypeBuilder<MarkerEntity> builder)
        {
            builder
                .ToTable("Marker")
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Description)
                .HasMaxLength(256);

            builder
                .Property(m => m.Kind)
                .IsRequired();

            builder
                .HasOne(marker => marker.Map)
                .WithMany(map => map.Markers)
                .HasForeignKey(marker => marker.MapId);

            builder
                .HasOne(marker => marker.Quest)
                .WithMany(quest => quest.Markers)
                .HasForeignKey(marker => marker.QuestId);

            builder
                .HasMany(marker => marker.Resources)
                .WithMany(res => res.Markers)
                .UsingEntity<PictureEntity>(
                    j => j
                    .HasOne(picture => picture.Resource)
                    .WithMany(res => res.Pictures)
                    .HasForeignKey(picture => picture.ResourceId),
                    j => j
                    .HasOne(picture => picture.Marker)
                    .WithMany(marker => marker.Pictures)
                    .HasForeignKey(picture => picture.MarkerId),
                    j =>
                    {
                        j.Property(picture => picture.Amount).HasDefaultValue(1);
                        j.HasKey(picture => new { picture.MarkerId, picture.ResourceId });
                        j.ToTable("Picture");
                    }                
                );

            builder
                .HasMany(marker => marker.Profiles)
                .WithMany(p => p.Markers)
                .UsingEntity<MarkerStateEntity>(
                    j => j
                    .HasOne(state => state.Profile)
                    .WithMany(p => p.MarkerStates)
                    .HasForeignKey(state => state.ProfileId),
                    j => j
                    .HasOne(state => state.Marker)
                    .WithMany(marker => marker.MarkerStates)
                    .HasForeignKey(state => state.MarkerId),
                    j =>
                    {
                        j.Property(state => state.IsSeleced).HasDefaultValue(false);
                        j.Property(state => state.IsFinished).HasDefaultValue(false);
                        j.HasKey(state => new { state.ProfileId, state.MarkerId });
                        j.ToTable("MarkerState");
                    }
                );

            //builder
            //    .HasMany(m => m.MarkerStates)
            //    .WithOne(state => state.Marker);
        }
    }
}