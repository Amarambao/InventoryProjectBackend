using CommonLayer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonLayer.Configuration
{
    public class InventoryTypeConfig : IEntityTypeConfiguration<InventoryTypeEntity>
    {
        public void Configure(EntityTypeBuilder<InventoryTypeEntity> builder)
        {
            builder.ToTable("inventory_type");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.NormalizedName)
                .IsUnique(true);

            builder.HasMany(x => x.Inventories)
                .WithOne(x => x.InventoryType)
                .HasForeignKey(x => x.InventoryTypeId);
        }
    }
}
