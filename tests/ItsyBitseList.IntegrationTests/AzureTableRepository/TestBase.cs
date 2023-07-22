using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.IntegrationTests.TestObjects;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist.GetItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;

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

        protected async Task<(string, WishListDetails, ItemDetails)> CreateAndAddItems(string itemName, Uri? uri = null)
        {
            (var response, var location) = await CreateWishlist("{\"name\":\"My Wishlist\"}");
            var content = uri == null ? $"{{\"details\":\"{itemName}\"}}" : $"{{\"details\":\"{itemName}\", \"link\":\"{uri}\"}}";
            var addItemResponse = await _client.PostAsync($"{location}/item", new StringContent(content, Encoding.UTF8, "application/json"));
            addItemResponse.EnsureSuccessStatusCode(); // Status Code 200-299
            string itemLocation = addItemResponse.Headers.Location.ToString();
            
            var itemResponse = await _client.GetAsync(itemLocation);
            var item = await itemResponse.Parse<ItemDetails>();

            var wishlistResponse = await _client.GetAsync(location);
            wishlistResponse.EnsureSuccessStatusCode(); // Status Code 200-299
            WishListDetails? wishlist = await wishlistResponse.Parse<WishListDetails>();

            return (itemLocation, wishlist, item);
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
