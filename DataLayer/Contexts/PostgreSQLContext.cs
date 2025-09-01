using CommonLayer.Configuration;
using CommonLayer.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Contexts
{
    public class PostgreSQLContext : IdentityDbContext<AppUserEntity, IdentityRole<Guid>, Guid>
    {
        public DbSet<InventoryEntity> Inventories { get; set; }
        public DbSet<InventoryTypeEntity> InventoryType { get; set; }
        public DbSet<ItemTypeEntity> ItemType { get; set; }
        public DbSet<InventoryItemTypesEntity> InventoryItemTypes { get; set; }
        public DbSet<InventoryEditorsEntity> InventoryEditors { get; set; }
        public DbSet<InventoryTagEntity> InventoryTags { get; set; }
        public DbSet<ChatMessageEntity> ChatMessages { get; set; }
        public DbSet<CustomIdElementSequenceEntity> CustomIdSequence { get; set; }
        public DbSet<StoredItemsEntity> StoredItems { get; set; }
        public DbSet<TagEntity> Tags { get; set; }

        public PostgreSQLContext() { }

        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AppUserConfig());
            modelBuilder.ApplyConfiguration(new ChatMessageConfig());
            modelBuilder.ApplyConfiguration(new CustomIdElementSequenceConfig());
            modelBuilder.ApplyConfiguration(new InventoryConfig());
            modelBuilder.ApplyConfiguration(new InventoryEditorsConfig());
            modelBuilder.ApplyConfiguration(new InventoryItemTypesConfig());
            modelBuilder.ApplyConfiguration(new InventoryTagConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryTypeConfig());
            modelBuilder.ApplyConfiguration(new ItemTypeConfig());
            modelBuilder.ApplyConfiguration(new StoredItemsConfig());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
        }
    }
}
