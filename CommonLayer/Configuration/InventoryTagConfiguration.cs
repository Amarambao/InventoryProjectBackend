using CommonLayer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonLayer.Configuration
{
    public class InventoryTagConfiguration : IEntityTypeConfiguration<InventoryTagEntity>
    {
        public void Configure(EntityTypeBuilder<InventoryTagEntity> builder)
        {
            builder.ToTable("inventory_tags");

            builder.HasKey(x => new { x.InventoryId, x.TagId });

            builder.HasOne(x => x.Inventory)
                .WithMany(x => x.InventoryTags)
                .HasForeignKey(x => x.InventoryId);

            builder.HasOne(x => x.Tag)
                .WithMany(x => x.InventoryTags)
                .HasForeignKey(x => x.TagId);
        }
    }
}
