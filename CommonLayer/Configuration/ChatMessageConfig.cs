using CommonLayer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonLayer.Configuration
{
    public class ChatMessageConfig : IEntityTypeConfiguration<ChatMessageEntity>
    {
        public void Configure(EntityTypeBuilder<ChatMessageEntity> builder)
        {
            builder.ToTable("chat");

            builder.HasKey(x => new { x.UserId, x.InventoryId, x.WrittenAt });

            builder.HasOne(x => x.Inventory)
                .WithMany(x => x.ChatMessages)
                .HasForeignKey(x => x.InventoryId);
        }
    }
}
