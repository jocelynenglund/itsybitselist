using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.Infrastructure.Persistence;
using System.Net.Http.Json;
using System.Text;

namespace ItsyBitseList.IntegrationTests.SeededRepository
{
    public class PublicWishlistEndpointTests : TestBase
    {
        public PublicWishlistEndpointTests(TestApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task CanPromiseItemsInExistingWishlist()
        {
            var response = await _client.PatchAsync($"/public/{InMemoryRepository.FirstId}/item/{InMemoryRepository.MovieCard}", new StringContent($"{{\"state\":\"{State.Promised}\"}}", Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CanRevertPromiseItemsInExistingWishlist()
        {
            var response = await _client.PatchAsync($"/public/{InMemoryRepository.FirstId}/item/{InMemoryRepository.MovieCard}", new StringContent($"{{\"state\":\"{State.Promised}\"}}", Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var promiseKey = await response.Content.ReadFromJsonAsync<Guid>();

            var revertResponse = await _client.PatchAsync($"/public/{InMemoryRepository.FirstId}/item/{InMemoryRepository.MovieCard}", new StringContent($"{{\"state\":\"{State.Wished}\", \"promiseKey\": \"{promiseKey}\"}}", Encoding.UTF8, "application/json"));

            revertResponse.EnsureSuccessStatusCode();
        }

    }
}
