using FluentAssertions;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ItsyBitseList.IntegrationTests.AzureTableRepository
{
    public class PublicWishlistEnpointTests : TestBase
    {
        public PublicWishlistEnpointTests(WebApplicationFactory<Program> factory) : base(factory) { }

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
            Cleanup();
        }
      


    }
}
