using CommonLayer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonLayer.Configuration
{
    public class InventoryItemTypesConfig : IEntityTypeConfiguration<InventoryItemTypesEntity>
    {
        public void Configure(EntityTypeBuilder<InventoryItemTypesEntity> builder)
        {
            builder.ToTable("inventory_item_types");

            builder.HasKey(x => new { x.ItemId, x.InventoryId });

            builder.HasOne(x => x.Inventory)
                .WithMany(x => x.InventoryItemTypes)
                .HasForeignKey(x => x.InventoryId);

            builder.HasOne(x => x.Item)
                .WithMany(x => x.InventoryItemTypes)
                .HasForeignKey(x => x.ItemId);

            builder.HasMany(x => x.CustomIdSequence)
                .WithOne(x => x.InventoryItemType)
                .HasForeignKey(x => new { x.ItemId, x.InventoryId });

            builder.HasMany(x => x.StoredItems)
                .WithOne(x => x.InventoryItemType)
                .HasForeignKey(x => new { x.ItemId, x.InventoryId });
        }
    }
}
