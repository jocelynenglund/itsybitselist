using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace ItsyBitseList.IntegrationTests
{
    public class TestApplicationFactory<TProgram> 
        : WebApplicationFactory<TProgram> 
        where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var repo = new InMemoryRepository(seeded: true);
                services.AddSingleton<IAsyncRepository<Wishlist>>((sp) => repo);
                services.AddSingleton<IWishlistRepository>((sp) => repo);
            });
        }
    }
}   
