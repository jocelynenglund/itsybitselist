using FluentAssertions;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.Infrastructure.Persistence;
using System.Net.Http.Json;
using System.Text;

namespace ItsyBitseList.IntegrationTests.SeededRepository
{
    public class WishlistEndpointTests : TestBase
    {
        public WishlistEndpointTests(TestApplicationFactory<Program> factory) : base(factory) { }

        [Theory]
        [InlineData("/wishlist")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            _client.DefaultRequestHeaders.Add("owner", "me");
            // Act
            var response = await _client.GetAsync(url);
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                               response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task CanCreateAWishlistAndNavigateToIt()
        {
            var response = await _client.PostAsync("/wishlist", new StringContent("{\"name\":\"My Wishlist\"}", Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            string location = response.Headers.Location.ToString();

            var wishlistResponse = await _client.GetAsync(location);
            wishlistResponse.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Fact]
        public async Task CanRetrieveExistingWishlist()
        {
            var firstList = await _client.GetAsync($"/wishlist/{InMemoryRepository.FirstId}");

            firstList.EnsureSuccessStatusCode(); // Status Code 200-299
        }


        [Fact]
        public async Task CanAddItemsToExistingWishlist()
        {
            var itemName = "My Wishlist Item";
            var response = await _client.PostAsync($"/wishlist/{InMemoryRepository.FirstId}/item", new StringContent($"{{\"details\":\"{itemName}\"}}", Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            string location = response.Headers.Location.ToString();

            var wishlistResponse = await _client.GetAsync(location);
            wishlistResponse.EnsureSuccessStatusCode(); // Status Code 200-299
            var item = await wishlistResponse.Content.ReadFromJsonAsync<WishlistItem>();
            item.Description.Should().Be(itemName);
            item.State.Should().Be(State.Wished);
        }


        [Fact]
        public async Task CanDeleteWishlist()
        {
            var response = await _client.PostAsync("/wishlist", new StringContent("{\"name\":\"My Wishlist\"}", Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            string location = response.Headers.Location.ToString();

            var deleteResponse = await _client.DeleteAsync(location);
            deleteResponse.EnsureSuccessStatusCode(); // Status Code 200-299

            var wishlistResponse = await _client.GetAsync(location);
            wishlistResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound); // Status Code 200-299
        }

    }
}
