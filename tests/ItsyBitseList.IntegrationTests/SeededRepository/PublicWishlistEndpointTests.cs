using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.IntegrationTests.TestObjects;
using System.Net.Http.Json;
using System.Text;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;

namespace ItsyBitseList.IntegrationTests.SeededRepository
{
    public class PublicWishlistEndpointTests : TestBase
    {
        public PublicWishlistEndpointTests(TestApplicationFactory<Program> factory) : base(factory) { }

        /// <summary>
        /// This test was created specifically to test the bug where the public wishlist endpoint would return a 404. 
        /// It happens when the Guid generated for the wishlist is encoded to a string that contains a / character.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CanAccessPublicUrl()
        {
            var wishlistResponse = await _client.GetAsync($"/wishlist/{InMemoryRepository.FirstId}");
            WishListDetails? wishlist = await wishlistResponse.Parse<WishListDetails>();
            var publicLocation = $"public/{wishlist.PublicId}";
            var publicResponse = await _client.GetAsync(publicLocation);

            publicResponse.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task CanAccessPublicItemUrl()
        {
            var wishlistResponse = await _client.GetAsync($"/wishlist/{InMemoryRepository.FirstId}");
            WishListDetails? wishlist = await  wishlistResponse.Parse<WishListDetails>();
            var publicLocation = $"public/{wishlist.PublicId}/item/{InMemoryRepository.MovieCard}";
            var publicResponse = await _client.GetAsync(publicLocation);

            publicResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CanPromiseItemsInExistingWishlist()
        {
            var wishlistResponse = await _client.GetAsync($"/wishlist/{InMemoryRepository.FirstId}");
            WishListDetails? wishlist = await wishlistResponse.Parse<WishListDetails>();
            var publicLocation = $"public/{wishlist.PublicId}/item/{InMemoryRepository.MovieCard}";
            var response = await _client.PatchAsync(publicLocation, new StringContent($"{{\"state\":\"{State.Promised}\"}}", Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CanRevertPromiseItemsInExistingWishlist()
        {
            var wishlistResponse = await _client.GetAsync($"/wishlist/{InMemoryRepository.FirstId}");
            WishListDetails? wishlist = await wishlistResponse.Parse<WishListDetails>();
            var publicLocation = $"public/{wishlist.PublicId}/item/{InMemoryRepository.MovieCard}";
            var response = await _client.PatchAsync(publicLocation, new StringContent($"{{\"state\":\"{State.Promised}\"}}", Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var promiseKey = await response.Content.ReadFromJsonAsync<Guid>();

            var revertResponse = await _client.PatchAsync(publicLocation, new StringContent($"{{\"state\":\"{State.Wished}\", \"promiseKey\": \"{promiseKey}\"}}", Encoding.UTF8, "application/json"));

            revertResponse.EnsureSuccessStatusCode();
        }
    }
}
