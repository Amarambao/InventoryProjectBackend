using CommonLayer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonLayer.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<TagEntity>
    {
        public void Configure(EntityTypeBuilder<TagEntity> builder)
        {
            builder.ToTable("tags");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.NormalizedName)
                .IsUnique(true);

            builder.HasMany(x => x.InventoryTags)
                .WithOne(x => x.Tag)
                .HasForeignKey(x => x.TagId);
        }
    }
}
