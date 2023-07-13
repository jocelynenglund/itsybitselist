using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace ItsyBitseList.Infrastructure
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            var repo = new InMemoryRepository(seeded: false);
            services.AddSingleton<IAsyncRepository<Wishlist>>(repo);
            services.AddSingleton<IWishlistRepository>((sp) => repo);
            return services;
        }


        public static IServiceCollection AddStorage(this IServiceCollection services)
        {
            services.AddTransient<IAsyncRepository<Wishlist>, AzureTableRepository>();
            services.AddScoped<IWishlistRepository, AzureTableRepository>();
            return services;
        }
    }

}
