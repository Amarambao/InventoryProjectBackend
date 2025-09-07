using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using DataLayer.Interfaces.Bogus;
using DataLayer.Repos;
using DataLayer.Repos.Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Settings
{
    public class DI
    {
        public static void Add(WebApplicationBuilder builder)
        {
            AddContext(builder.Services, builder.Configuration);
            AddIdentity(builder.Services, builder.Configuration);
            IdentityPaswordSettings(builder.Services);
            AddService(builder.Services);
        }

        private static void AddContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionSection = configuration.GetConnectionString("IdentityDatabase");

            services.AddDbContext<PostgreSQLContext>(
                    options => options.UseNpgsql(configuration.GetConnectionString("IdentityDatabase")));
        }

        private static void AddIdentity(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUserEntity, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<PostgreSQLContext>()
                .AddApiEndpoints();
        }

        private static void IdentityPaswordSettings(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;
            });
        }

        private static void AddService(IServiceCollection services)
        {
            services.AddScoped<IChatMessagesRepo, ChatMessagesRepo>();
            services.AddScoped<ICheckRepo, CheckRepo>();
            services.AddScoped<ICustomDescriptionSequenceRepo, CustomDescriptionSequenceRepo>();
            services.AddScoped<ICustomIdElementSequenceRepo, CustomIdElementSequenceRepo>();
            services.AddScoped<IInventoryEditorsRepo, InventoryEditorsRepo>();
            services.AddScoped<IInventoryItemTypesRepo, InventoryItemTypesRepo>();
            services.AddScoped<IInventoryRepo, InventoryRepo>();
            services.AddScoped<IInventoryTagRepo, InventoryTagRepo>();
            services.AddScoped<IInventoryTypeRepo, InventoryTypeRepo>();
            services.AddScoped<IItemTypeRepo, ItemTypeRepo>();
            services.AddScoped<IStoredItemsRepo, StoredItemsRepo>();
            services.AddScoped<ITagRepo, TagRepo>();

            services.AddScoped<IBogusGenerationRepo, BogusGenerationRepo>();
        }
    }
}
