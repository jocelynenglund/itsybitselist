using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.Infrastructure.Persistence;
using ItsyBitseList.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ItsyBitseList.Infrastructure
{
    public static class InfrastructureDependencies
    {

        public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configurationSection)
        {
            services.Configure<StorageSettings>(configurationSection);
            services.AddTransient<IAsyncRepository<Wishlist>, AzureTableRepository>();
            services.AddScoped<IWishlistRepository, AzureTableRepository>();
            return services;
        }
    }

}
