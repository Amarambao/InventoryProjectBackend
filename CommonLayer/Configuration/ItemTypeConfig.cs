using CommonLayer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonLayer.Configuration
{
    public class ItemTypeConfig : IEntityTypeConfiguration<ItemTypeEntity>
    {
        public void Configure(EntityTypeBuilder<ItemTypeEntity> builder)
        {
            builder.ToTable("item_type");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.NormalizedName)
                .IsUnique(true);

            builder.HasMany(x => x.InventoryItemTypes)
                .WithOne(x => x.Item)
                .HasForeignKey(x => x.ItemId);
        }
    }
}
