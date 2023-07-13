using FluentAssertions;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ItsyBitseList.IntegrationTests
{
    public class PersistenceTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private List<string> locations = new List<string>();
        public PersistenceTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CanCreateAWishlistAndNavigateToIt()
        {
            (var response, var location) = await CreateWishlist("{\"name\":\"My Wishlist\"}");
            response.EnsureSuccessStatusCode();
            var wishlistResponse = await _client.GetAsync(location);
            wishlistResponse.EnsureSuccessStatusCode(); // Status Code 200-299
            Cleanup();
        }

        [Fact]
        public async Task CanDeleteWishList()
        {
            (var response, var location) = await CreateWishlist("{\"name\":\"My Wishlist\"}", skipCleanup: true);

            var deleteResponse = await _client.DeleteAsync(location);
            deleteResponse.EnsureSuccessStatusCode(); // Status Code 200-299

            var wishlistResponse = await _client.GetAsync(location);
            wishlistResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CanAddItemsToWishlist()
        {
            var itemName = "My Wishlist Item";

            (var location, var wishlist ) = await CreateAndAddItems(itemName);
            wishlist.Items.Should().Contain(i => i.Description == itemName);
            Cleanup();
        }

        [Fact]
        public async Task CanPromiseItem()
        {
            (var location, var wishlist) = await CreateAndAddItems("My Wishlist Item");
            HttpResponseMessage response = await PromiseItem(location);
            var itemResponse = await _client.GetAsync(location);
            var item = await Parse<ItemDetails>(itemResponse);
            
            response.EnsureSuccessStatusCode();
            item.State.Should().Be(State.Promised);

            Cleanup();
        }

        [Fact]
        public async Task CanRevertPromise()
        {
            (var location, _) = await CreateAndAddItems("My Wishlist Item");
            HttpResponseMessage response = await PromiseItem(location);

            var promiseKey = await response.Content.ReadAsStringAsync();
            response = await RevertPromiseItem(location, promiseKey);

            var itemResponse = await _client.GetAsync(location);
            var item = await Parse<ItemDetails>(itemResponse);

            response.EnsureSuccessStatusCode();
            item.State.Should().Be(State.Wished);
        }
        [Fact]
        public async Task CanDeleteItem()
        {
            (var location, _) = await CreateAndAddItems("My Wishlist Item");
            
            HttpResponseMessage deleteResponse = await _client.DeleteAsync(location);
            var itemResponse = await _client.GetAsync(location);

            deleteResponse.EnsureSuccessStatusCode();
            itemResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }

        private async Task<HttpResponseMessage> PromiseItem(string location)
        {
            return await _client.PatchAsync($"{location}", new StringContent($"{{\"state\":\"{State.Promised}\"}}", Encoding.UTF8, "application/json"));
        }

        private async Task<HttpResponseMessage> RevertPromiseItem(string location, string promiseKey)
        {
            return await _client.PatchAsync($"{location}", new StringContent($"{{\"state\":\"{State.Wished}\",\"promiseKey\":{promiseKey}}}", Encoding.UTF8, "application/json"));
        }

        private async Task<(string, WishListDetails)> CreateAndAddItems(string itemName)
        {
            (var response, var location) = await CreateWishlist("{\"name\":\"My Wishlist\"}");

            var itemResponse = await _client.PostAsync($"{location}/item", new StringContent($"{{\"details\":\"{itemName}\"}}", Encoding.UTF8, "application/json"));
            itemResponse.EnsureSuccessStatusCode(); // Status Code 200-299
            string itemLocation = itemResponse.Headers.Location.ToString();

            var wishlistResponse = await _client.GetAsync(location);
            wishlistResponse.EnsureSuccessStatusCode(); // Status Code 200-299
            WishListDetails? wishlist = await Parse<WishListDetails>(wishlistResponse);

            return (itemLocation, wishlist);
        }
       
        private static async Task<T> Parse<T>(HttpResponseMessage wishlistResponse)
        {

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter(allowIntegerValues: true) },
                PropertyNameCaseInsensitive = true
            };
            return await wishlistResponse.Content.ReadFromJsonAsync<T>(options);
        }


        private async Task<(HttpResponseMessage, string)> CreateWishlist(string content, bool skipCleanup = false)
        {
            var response = await _client.PostAsync("/wishlist", new StringContent(content, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            string location = response.Headers.Location.ToString();
            if (!skipCleanup) locations.Add(location);
            return (response, location);
        }

        private void Cleanup()
        {
            foreach (var item in locations)
            {
                _client.DeleteAsync(item);
            }
        }
    }
}
