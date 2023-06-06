using ItsyBitseList.Core.Interfaces;
using ItsyBitseList.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ItsyBitseList.Infrastructure
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IWishlistCollectionRepository, InMemoryRepository>();
            return services;
        }

    }

}
