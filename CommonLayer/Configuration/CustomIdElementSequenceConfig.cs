using CommonLayer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonLayer.Configuration
{
    public class CustomIdElementSequenceConfig : IEntityTypeConfiguration<CustomIdElementSequenceEntity>
    {
        public void Configure(EntityTypeBuilder<CustomIdElementSequenceEntity> builder)
        {
            builder.ToTable("custom_id_sequence");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.InventoryId, x.ItemId })
                .IsUnique(false);

            builder.Property(x => x.ElementType)
                .HasConversion<int>() 
                .IsRequired();

            builder.Property(x => x.FixedTextValue)
                .HasMaxLength(256)
                .IsUnicode(true);

            builder.HasOne(x => x.InventoryItemType)
                .WithMany(x => x.CustomIdSequence)
                .HasForeignKey(x => new { x.InventoryId, x.ItemId });
        }
    }
}
