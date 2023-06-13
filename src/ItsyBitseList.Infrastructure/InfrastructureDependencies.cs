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
            var repo = new InMemoryRepository();
            services.AddSingleton<IAsyncRepository<Wishlist>>(repo);
            services.AddSingleton<IAsyncRepository<WishlistItem>>(repo);
            services.AddSingleton<IWishlistRepository>(repo);
            return services;
        }

    }

}
