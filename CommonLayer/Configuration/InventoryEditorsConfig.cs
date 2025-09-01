using CommonLayer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonLayer.Configuration
{
    public class InventoryEditorsConfig : IEntityTypeConfiguration<InventoryEditorsEntity>
    {
        public void Configure(EntityTypeBuilder<InventoryEditorsEntity> builder)
        {
            builder.ToTable("inventory_editors");

            builder.HasKey(x => new { x.UserId, x.InventoryId });

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserInventories)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Inventory)
                .WithMany(x => x.InventoryEditors)
                .HasForeignKey(x => x.InventoryId);
        }
    }
}
