using CommonLayer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonLayer.Configuration
{
    public class CustomDescriptionSequenceConfig : IEntityTypeConfiguration<CustomDescriptionSequenceEntity>
    {
        public void Configure(EntityTypeBuilder<CustomDescriptionSequenceEntity> builder)
        {
            builder.ToTable("custom_description_sequence");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.InventoryId, x.ItemId })
                .IsUnique(false);

            builder.Property(x => x.DescripionType)
                .HasConversion<int>()
                .IsRequired();

            builder.HasOne(x => x.InventoryItemType)
                .WithMany(x => x.CustomDescriptionSequence)
                .HasForeignKey(x => new { x.InventoryId, x.ItemId });
        }
    }
}
