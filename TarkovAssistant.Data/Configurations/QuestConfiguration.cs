using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Data.Configurations
{
    public class QuestConfiguration : IEntityTypeConfiguration<QuestEntity>
    {
        public void Configure(EntityTypeBuilder<QuestEntity> builder)
        {
            builder
                .ToTable("Quest")
                .HasKey(q => q.Id);

            builder
                .Property(q => q.Name)
                .IsRequired()
                .HasMaxLength(64);

            builder
                .HasMany(quest => quest.Markers)
                .WithOne(marker => marker.Quest);
        }
    }
}
