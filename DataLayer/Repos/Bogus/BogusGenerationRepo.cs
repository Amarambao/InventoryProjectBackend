using Bogus;
using CommonLayer.Enum;
using CommonLayer.Extensions;
using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces.Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DataLayer.Repos.Bogus
{
    public class BogusGenerationRepo : IBogusGenerationRepo
    {
        private readonly PostgreSQLContext _context;
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public BogusGenerationRepo(
            PostgreSQLContext context,
            UserManager<AppUserEntity> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task CreateAdminAsync()
        {
            if (!await _roleManager.RoleExistsAsync("admin"))
                await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = "admin" });

            var adminUser = await _userManager.FindByEmailAsync("admin@admin.com");

            if (adminUser is null)
            {
                adminUser = new AppUserEntity
                {
                    UserName = "Admin",
                    Email = "admin@admin.com",
                    Name = "Admin",
                    NormalizedName = "Admin".CustomNormalize(),
                };
                await _userManager.CreateAsync(adminUser, "admin");
            }

            if (!await _userManager.IsInRoleAsync(adminUser, "admin"))
                await _userManager.AddToRoleAsync(adminUser, "admin");
        }

        public async Task CreateMainDataAsync()
        {
            var users = await CreateUsersAsync();
            //var users = await _context.Users.Where(u => u.NormalizedName != "ADMIN").ToListAsync();

            var faker = new Faker();

            var inventoryTypes = await CreateInventoryTypesAsync(faker);
            //var inventoryTypes = await _context.InventoryType.ToListAsync();

            var itemTypes = await CreateItemTypesAsync(faker);
            //var itemTypes = await _context.ItemType.ToListAsync();

            var tags = await CreateTagsAsync(faker);
            //var tags = await _context.Tags.ToListAsync();

            foreach (var user in users)
                await CreateInventoriesAsync(faker, user, users, inventoryTypes, itemTypes, tags);

            await _context.SaveChangesAsync();
        }

        private async Task<List<AppUserEntity>> CreateUsersAsync()
        {
            Randomizer.Seed = new Random(RandomNumberGenerator.GetInt32(int.MaxValue));

            var testUsers = new Faker<AppUserEntity>()
                .CustomInstantiator(f => new AppUserEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = string.Empty,
                    NormalizedName = string.Empty,
                    AccessFailedCount = 0,
                    EmailConfirmed = false,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = string.Empty,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    IsBlocked = false,
                    LockoutEnabled = false,
                    LockoutEnd = null
                })
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.NormalizedName, (f, u) => u.Name.CustomNormalize())
                .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.Name))
                .RuleFor(u => u.NormalizedUserName, (f, u) => _userManager.NormalizeName(u.UserName))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.UserName))
                .RuleFor(u => u.NormalizedEmail, (f, u) => _userManager.NormalizeEmail(u.Email))
                .FinishWith((f, u) =>
                {
                    Console.WriteLine($"User Created! Id={u.Id}, UserName={u.UserName}");
                });

            var maxCount = 40;
            var created = 0;

            while (created < maxCount)
            {
                var user = testUsers.Generate();

                var identityResult = await _userManager.CreateAsync(user, "1");
                if (identityResult.Succeeded)
                    created++;
            }

            var result = await _context.Users.Where(u => u.NormalizedName != "ADMIN").ToListAsync();

            return result;
        }

        private async Task<List<InventoryTypeEntity>> CreateInventoryTypesAsync(Faker faker)
        {
            faker.Random = new Randomizer(RandomNumberGenerator.GetInt32(int.MaxValue));

            var commerceCategories = faker.Commerce.Categories(80);

            var inventoryTypes = commerceCategories
                .Distinct()
                .Select(c => new InventoryTypeEntity(c))
                .ToList();

            await _context.InventoryType.AddRangeAsync(inventoryTypes);
            await _context.SaveChangesAsync();

            return await _context.InventoryType.ToListAsync();
        }

        private async Task<List<ItemTypeEntity>> CreateItemTypesAsync(Faker faker)
        {
            var productTypes = new List<string>();

            faker.Random = new Randomizer(RandomNumberGenerator.GetInt32(int.MaxValue));

            for (int i = 0; i < 160; i++)
                productTypes.Add(faker.Commerce.ProductName());

            var itemTypes = productTypes
                .Distinct()
                .Select(p => new ItemTypeEntity(p))
                .ToList();

            await _context.ItemType.AddRangeAsync(itemTypes);
            await _context.SaveChangesAsync();

            return await _context.ItemType.ToListAsync();
        }

        private async Task<List<TagEntity>> CreateTagsAsync(Faker faker)
        {
            var words = faker.Random.WordsArray(640);

            var tags = words
                .Select(w => new TagEntity(w))
                .GroupBy(t => t.NormalizedName)
                    .Select(g => g.First())
                .ToList();

            await _context.Tags.AddRangeAsync(tags);
            await _context.SaveChangesAsync();

            return await _context.Tags.ToListAsync();
        }

        private async Task CreateInventoriesAsync(Faker faker, AppUserEntity creator, List<AppUserEntity> users, List<InventoryTypeEntity> inventoryTypes, List<ItemTypeEntity> itemTypes, List<TagEntity> tags)
        {
            for (int i = 0; i < RandomNumberGenerator.GetInt32(3); i++)
            {
                faker.Random = new Randomizer(RandomNumberGenerator.GetInt32(int.MaxValue));
                var waffle = faker.Waffle();

                var inventory = new InventoryEntity()
                {
                    Id = Guid.NewGuid(),
                    InventoryTypeId = faker.PickRandom(inventoryTypes).Id,
                    Description = waffle.Text(faker.Random.Number(3), false),
                    IsPublic = faker.Random.Bool()
                };

                await _context.Inventories.AddAsync(inventory);
                await _context.SaveChangesAsync();

                await AddInventoryEditorsAsync(faker, inventory, creator.Id, users);

                await AddInventoryTagsAsync(faker, inventory, tags);

                await AddInventoryItemTypes(faker, inventory, itemTypes);
            }
        }

        private async Task AddInventoryEditorsAsync(Faker faker, InventoryEntity inventory, Guid creatorId, List<AppUserEntity> users)
        {
            var pickedUsers = new HashSet<Guid> { creatorId };

            if (!inventory.IsPublic)
            {
                var maxExtra = faker.Random.Number(3);
                var candidates = users.Where(u => u.Id != creatorId).ToList();

                for (int i = 0; i < maxExtra && candidates.Any(); i++)
                {
                    var picked = faker.PickRandom(candidates);
                    pickedUsers.Add(picked.Id);
                    candidates.Remove(picked);
                }
            }

            var inventoryEditors = pickedUsers
                .Select(uid => new InventoryEditorsEntity
                {
                    InventoryId = inventory.Id,
                    UserId = uid,
                    IsCreator = uid == creatorId
                })
                .ToList();

            await _context.InventoryEditors.AddRangeAsync(inventoryEditors);
            await _context.SaveChangesAsync();

            var targetUsers = inventory.IsPublic ? users : users.Where(u => inventoryEditors.Any(e => e.UserId == u.Id)).ToList();

            if (!targetUsers.Any())
                targetUsers.Add(users.First(u => u.Id == creatorId)!);

            await AddChatMessagesAsync(faker, inventory, targetUsers);
        }

        private async Task AddChatMessagesAsync(Faker faker, InventoryEntity inventory, List<AppUserEntity> users)
        {
            if (!users.Any())
                return;

            var messages = new List<ChatMessageEntity>();

            for (int i = 0; i < faker.Random.Number(50); i++)
            {
                faker.Random = new Randomizer(RandomNumberGenerator.GetInt32(int.MaxValue));

                var pickedUser = faker.PickRandom(users);

                messages.Add(new ChatMessageEntity
                {
                    InventoryId = inventory.Id,
                    UserId = pickedUser.Id,
                    UserName = pickedUser.UserName ?? string.Empty,
                    WrittenAt = faker.Date.Between(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow),
                    Message = faker.Waffle().Text(1, false),
                });
            }

            await _context.ChatMessages.AddRangeAsync(messages);
            await _context.SaveChangesAsync();
        }

        private async Task AddInventoryTagsAsync(Faker faker, InventoryEntity inventory, List<TagEntity> tags)
        {
            var pickedTagIds = new HashSet<Guid>();
            var inventoryTags = new List<InventoryTagEntity>();

            var maxTags = faker.Random.Number(25);
            var tagPool = tags.ToList();

            for (int i = 0; i < maxTags && tagPool.Any(); i++)
            {
                var tag = faker.PickRandom(tagPool);
                tagPool.Remove(tag);

                inventoryTags.Add(new InventoryTagEntity
                {
                    InventoryId = inventory.Id,
                    TagId = tag.Id
                });
            }

            await _context.InventoryTags.AddRangeAsync(inventoryTags);
            await _context.SaveChangesAsync();
        }

        private async Task AddInventoryItemTypes(Faker faker, InventoryEntity inventory, List<ItemTypeEntity> items)
        {
            if (!items.Any())
                return;

            var inventoryItems = new List<InventoryItemTypesEntity>();

            var availableItems = items;
            var count = Math.Min(faker.Random.Number(15), availableItems.Count);

            for (int i = 0; i < count; i++)
            {
                var item = faker.PickRandom(availableItems);
                availableItems.Remove(item);

                inventoryItems.Add(new InventoryItemTypesEntity
                {
                    InventoryId = inventory.Id,
                    ItemId = item.Id,
                });
            }

            await _context.InventoryItemTypes.AddRangeAsync(inventoryItems);
            await _context.SaveChangesAsync();

            await AddItemCustomIdSequenceAsync(faker, inventoryItems);
        }

        private async Task AddItemCustomIdSequenceAsync(Faker faker, List<InventoryItemTypesEntity> inventoryItems)
        {
            foreach (var item in inventoryItems)
            {
                faker.Random = new Randomizer(RandomNumberGenerator.GetInt32(int.MaxValue));

                var cutomIdSequence = new List<CustomIdElementSequenceEntity>();

                var elementCount = faker.Random.Number(Enum.GetValues<CustomIdElementEnum>().Length);

                var selectedElements = faker.PickRandom(Enum.GetValues<CustomIdElementEnum>(), elementCount).ToList();

                var seenUIntSequence = false;
                selectedElements = selectedElements.Where(e =>
                {
                    if (e != CustomIdElementEnum.UIntSequence) return true;
                    if (seenUIntSequence) return false;
                    seenUIntSequence = true;
                    return true;
                }).ToList();

                for (int i = 0; i < selectedElements.Count; i++)
                {
                    var elementType = selectedElements[i];

                    cutomIdSequence.Add(new CustomIdElementSequenceEntity
                    {
                        Id = Guid.NewGuid(),
                        InventoryId = item.InventoryId,
                        ItemId = item.ItemId,
                        ElementType = elementType,
                        Order = i,
                        FixedTextValue = elementType == CustomIdElementEnum.FixedText ? faker.Random.AlphaNumeric(faker.Random.Number(9)) : null,
                        InventoryItemType = item
                    });
                }
                await _context.CustomIdSequence.AddRangeAsync(cutomIdSequence);
                await _context.SaveChangesAsync();

                await AddStoredItemsAsync(faker, item, cutomIdSequence);
            }
        }

        private async Task AddStoredItemsAsync(Faker faker, InventoryItemTypesEntity inventoryItem, List<CustomIdElementSequenceEntity> customIdSequence)
        {
            var storedItems = new List<StoredItemsEntity>();

            for (int i = 0; i < faker.Random.Number(15); i++)
            {
                faker.Random = new Randomizer(RandomNumberGenerator.GetInt32(int.MaxValue));
                storedItems.Add(new StoredItemsEntity
                {
                    Id = Guid.NewGuid(),
                    InventoryId = inventoryItem.InventoryId,
                    ItemId = inventoryItem.ItemId,
                    CustomId = GenerateCustomId(faker, customIdSequence, i)
                });
            }

            await _context.StoredItems.AddRangeAsync(storedItems);
            await _context.SaveChangesAsync();
        }

        private string GenerateCustomId(Faker faker, List<CustomIdElementSequenceEntity> customIdSequence, int UIntSequenceValue)
        {
            var sb = new StringBuilder();

            foreach (var element in customIdSequence)
            {
                switch (element.ElementType)
                {
                    case CustomIdElementEnum.FixedText:
                        sb.Append($"{element.FixedTextValue ?? string.Empty}-");
                        break;
                    case CustomIdElementEnum.Random20Bit:
                        sb.Append($"{faker.Random.UInt(0, 1 << 20)}-");
                        break;
                    case CustomIdElementEnum.Random32Bit:
                        sb.Append($"{BitConverter.ToUInt32(faker.Random.Bytes(4))}-");
                        break;
                    case CustomIdElementEnum.Random6Digit:
                        sb.Append($"{faker.Random.Number(1000000)}-");
                        break;
                    case CustomIdElementEnum.Random9Digit:
                        sb.Append($"{faker.Random.Number(1000000000)}-");
                        break;
                    case CustomIdElementEnum.Guid:
                        sb.Append($"{faker.Random.Guid()}-");
                        break;
                    case CustomIdElementEnum.DateTime:
                        sb.Append($"{faker.Date.Between(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow)}-");
                        break;
                    case CustomIdElementEnum.UIntSequence:
                        sb.Append(UIntSequenceValue);
                        break;
                }
            }
            return sb.ToString().Trim('-');
        }
    }
}
