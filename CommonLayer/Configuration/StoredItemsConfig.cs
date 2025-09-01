using CommonLayer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonLayer.Configuration
{
    public class StoredItemsConfig : IEntityTypeConfiguration<StoredItemsEntity>
    {
        public void Configure(EntityTypeBuilder<StoredItemsEntity> builder)
        {
            builder.ToTable("stored_items");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.ItemId, x.InventoryId })
                .IsUnique(false);

            builder.HasOne(x => x.InventoryItemType)
                .WithMany(x => x.StoredItems)
                .HasForeignKey(x => new { x.ItemId, x.InventoryId });
        }
    }
}
