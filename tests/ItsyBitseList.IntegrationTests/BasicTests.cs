using FluentAssertions;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.Infrastructure.Persistence;
using System.Net.Http.Json;
using System.Text;

namespace ItsyBitseList.IntegrationTests
{
    public class BasicTests : IClassFixture<TestApplicationFactory<Program>>
    {
        private readonly TestApplicationFactory<Program> _factory;
        private HttpClient _client;
        public BasicTests(TestApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

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
        public async Task CanPromiseItemsInExistingWishlist()
        { 
            var response = await _client.PatchAsync($"/wishlist/{InMemoryRepository.FirstId}/item/{InMemoryRepository.MovieCard}", new StringContent($"{{\"state\":\"{State.Promised}\"}}", Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CanRevertPromiseItemsInExistingWishlist()
        {
            var response = await _client.PatchAsync($"/wishlist/{InMemoryRepository.FirstId}/item/{InMemoryRepository.MovieCard}", new StringContent($"{{\"state\":\"{State.Promised}\"}}", Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var promiseKey = await response.Content.ReadFromJsonAsync<Guid>();

            var revertResponse = await _client.PatchAsync($"/wishlist/{InMemoryRepository.FirstId}/item/{InMemoryRepository.MovieCard}", new StringContent($"{{\"state\":\"{State.Wished}\", \"promiseKey\": \"{promiseKey}\"}}", Encoding.UTF8, "application/json"));

            revertResponse.EnsureSuccessStatusCode();
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
