using FluentAssertions;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.IntegrationTests.TestObjects;
using Microsoft.AspNetCore.Mvc.Testing;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist.GetItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;

namespace ItsyBitseList.IntegrationTests.AzureTableRepository
{
    public class PublicWishlistEnpointTests : TestBase
    {
        public PublicWishlistEnpointTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task CanAccessPublicUrl()
        {
            (var response, var location) = await CreateWishlist("{\"name\":\"My Wishlist\"}");
            response.EnsureSuccessStatusCode();
            var wishlistResponse = await _client.GetAsync(location);
            WishListDetails? wishlist = await wishlistResponse.Parse<WishListDetails>();
            var publicLocation = $"public/{wishlist.PublicId}";
            var publicResponse = await _client.GetAsync(publicLocation);
            publicResponse.EnsureSuccessStatusCode();

            Cleanup();
        }
        [Fact]
        public async Task CanPromiseItem()
        {
            (var location, var wishlist, var item) = await CreateAndAddItems("My Wishlist Item");
            location = $"public/{wishlist.PublicId}/item/{item.Id}";
            HttpResponseMessage response = await PromiseItem(location);
            var itemResponse = await _client.GetAsync(location);
            item = await itemResponse.Parse<ItemDetails>();
            
            response.EnsureSuccessStatusCode();
            item.State.Should().Be(State.Promised);

            Cleanup();
        }

        [Fact]
        public async Task CanRevertPromise()
        {
            (var location, var wishlist, var item) = await CreateAndAddItems("My Wishlist Item");
            location = $"public/{wishlist.PublicId}/item/{item.Id}";
            HttpResponseMessage response = await PromiseItem(location);

            var promiseKey = await response.Content.ReadAsStringAsync();
            response = await RevertPromiseItem(location, promiseKey);

            var itemResponse = await _client.GetAsync(location);
            item = await itemResponse.Parse<ItemDetails>();

            response.EnsureSuccessStatusCode();
            item.State.Should().Be(State.Wished);
            Cleanup();
        }
      


    }
}
