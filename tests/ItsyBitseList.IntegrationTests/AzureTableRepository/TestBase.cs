using ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ItsyBitseList.IntegrationTests.AzureTableRepository
{
    public class TestBase : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly WebApplicationFactory<Program> _factory;
        protected HttpClient _client;
        protected List<string> locations = new List<string>();
        public TestBase(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        protected async Task<HttpResponseMessage> PromiseItem(string location)
        {
            return await _client.PatchAsync($"{location}", new StringContent($"{{\"state\":\"{State.Promised}\"}}", Encoding.UTF8, "application/json"));
        }

        protected async Task<HttpResponseMessage> RevertPromiseItem(string location, string promiseKey)
        {
            return await _client.PatchAsync($"{location}", new StringContent($"{{\"state\":\"{State.Wished}\",\"promiseKey\":{promiseKey}}}", Encoding.UTF8, "application/json"));
        }

        protected async Task<(string, WishListDetails)> CreateAndAddItems(string itemName, Uri? uri = null)
        {
            (var response, var location) = await CreateWishlist("{\"name\":\"My Wishlist\"}");
            var content = uri == null ? $"{{\"details\":\"{itemName}\"}}" : $"{{\"details\":\"{itemName}\", \"link\":\"{uri}\"}}";
            var itemResponse = await _client.PostAsync($"{location}/item", new StringContent(content, Encoding.UTF8, "application/json"));
            itemResponse.EnsureSuccessStatusCode(); // Status Code 200-299
            string itemLocation = itemResponse.Headers.Location.ToString();

            var wishlistResponse = await _client.GetAsync(location);
            wishlistResponse.EnsureSuccessStatusCode(); // Status Code 200-299
            WishListDetails? wishlist = await Parse<WishListDetails>(wishlistResponse);

            return (itemLocation, wishlist);
        }

        protected static async Task<T> Parse<T>(HttpResponseMessage wishlistResponse)
        {

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter(allowIntegerValues: true) },
                PropertyNameCaseInsensitive = true
            };
            return await wishlistResponse.Content.ReadFromJsonAsync<T>(options);
        }


        protected async Task<(HttpResponseMessage, string)> CreateWishlist(string content, bool skipCleanup = false)
        {
            var response = await _client.PostAsync("/wishlist", new StringContent(content, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            string location = response.Headers.Location.ToString();
            if (!skipCleanup) locations.Add(location);
            return (response, location);
        }

        protected void Cleanup()
        {
            foreach (var item in locations)
            {
                _client.DeleteAsync(item);
            }
        }
    }
}
