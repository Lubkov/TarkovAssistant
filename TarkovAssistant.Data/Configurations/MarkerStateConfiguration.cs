using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Data.Configurations
{
    public class MarkerStateConfiguration : IEntityTypeConfiguration<MarkerStateEntity>
    {
        public void Configure(EntityTypeBuilder<MarkerStateEntity> builder)
        {
            builder.ToTable("MarkerState");
            builder.HasKey(state => new { state.ProfileId, state.MarkerId });
            
            builder.Property(state => state.IsSeleced)
               .HasDefaultValue(false);
            
            builder.Property(state => state.IsFinished)
               .HasDefaultValue(false);                       

            builder
                .HasOne(state => state.Profile)
                .WithMany(profile => profile.MarkerStates)
                .HasForeignKey(state => state.ProfileId);

            builder
                .HasOne(state => state.Marker)
                .WithMany(marker => marker.MarkerStates)
                .HasForeignKey(state => state.MarkerId);
        }
    }
}
