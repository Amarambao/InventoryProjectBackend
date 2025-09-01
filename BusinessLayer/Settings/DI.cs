using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.Bogus;
using BusinessLayer.Services;
using BusinessLayer.Services.Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Settings
{
    public class DI
    {
        public static void Add(WebApplicationBuilder builder)
        {
            DataLayer.Settings.DI.Add(builder);
            AddService(builder.Services);
        }

        private static void AddService(IServiceCollection services)
        {
            services.AddScoped<IAppUserSrv, AppUserSrv>();
            services.AddScoped<IChatMessagesSrv, ChatMessagesSrv>();
            services.AddScoped<ICheckSrv, CheckSrv>();
            services.AddScoped<ICustomIdElementSequenceSrv, CustomIdElementSequenceSrv>();
            services.AddScoped<IInventoryEditorsSrv, InventoryEditorsSrv>();
            services.AddScoped<IInventoryItemTypesSrv, InventoryItemTypesSrv>();
            services.AddScoped<IInventorySrv, InventorySrv>();
            services.AddScoped<IInventoryTagSrv, InventoryTagSrv>();
            services.AddScoped<IInventoryTypeSrv, InventoryTypeSrv>();
            services.AddScoped<IItemTypeSrv, ItemTypeSrv>();
            services.AddScoped<IJwtSrv, JwtSrv>();
            services.AddScoped<IStoredItemsSrv, StoredItemsSrv>();
            services.AddScoped<ITagSrv, TagSrv>();
            services.AddScoped<IUserOperationsSrv, UserOperationsSrv>();

            services.AddScoped<IBogusGenerationSrv, BogusGenerationSrv>();
        }
    }
}
