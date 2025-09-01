using CommonLayer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonLayer.Configuration
{
    public class InventoryConfig : IEntityTypeConfiguration<InventoryEntity>
    {
        public void Configure(EntityTypeBuilder<InventoryEntity> builder)
        {
            builder.ToTable("inventory");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.InventoryType)
                .WithMany(x => x.Inventories)
                .HasForeignKey(x => x.InventoryTypeId);
            
            builder.HasMany(x => x.InventoryEditors)
                .WithOne(x => x.Inventory)
                .HasForeignKey(x => x.InventoryId);

            builder.HasMany(x => x.InventoryItemTypes)
                .WithOne(x => x.Inventory)
                .HasForeignKey(x => x.InventoryId);

            builder.HasMany(x => x.InventoryTags)
                .WithOne(x => x.Inventory)
                .HasForeignKey(x => x.InventoryId);

            builder.HasMany(x => x.ChatMessages)
                .WithOne(x => x.Inventory)
                .HasForeignKey(x => x.InventoryId);
        }
    }
}
