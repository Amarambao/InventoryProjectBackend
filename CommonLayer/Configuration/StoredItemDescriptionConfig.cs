using CommonLayer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonLayer.Configuration
{
    public class StoredItemDescriptionConfig : IEntityTypeConfiguration<StoredItemDescriptionEntity>
    {
        public void Configure(EntityTypeBuilder<StoredItemDescriptionEntity> builder)
        {
            builder.ToTable("stored_item_descriptions");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.StoredItemId)
                .IsUnique(false);

            builder.Property(x => x.DescriptionType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.ShortText)
                .HasMaxLength(150)
                .IsUnicode(true);

            builder.Property(x => x.LongText)
                .HasMaxLength(1000)
                .IsUnicode(true);

            builder.Property(x => x.LongText)
                .HasMaxLength(2000)
                .IsUnicode(true);

            builder.HasOne(x => x.StoredItem)
                .WithMany(x => x.StoredItemDescriptions)
                .HasForeignKey(x => x.StoredItemId);
        }
    }
}
