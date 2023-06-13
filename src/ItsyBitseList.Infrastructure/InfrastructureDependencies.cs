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
            services.AddSingleton<IAsyncRepository<Wishlist>, InMemoryRepository>();
            services.AddSingleton<IAsyncRepository<WishlistItem>, InMemoryRepository>();
            services.AddSingleton<IWishlistRepository, InMemoryRepository>();
            return services;
        }

    }

}
