using FluentAssertions;
using ItsyBitseList.IntegrationTests.TestObjects;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;

namespace ItsyBitseList.IntegrationTests.AzureTableRepository
{
    public class WishlistEndpointTests : TestBase
    {
        private const string description = "description";
        public WishlistEndpointTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task CanCreateAWishlistAndNavigateToIt()
        {

            (var response, var location) = await CreateWishlist($"{{\"name\":\"My Wishlist\", \"description\":\"{description}\"}}");
            response.EnsureSuccessStatusCode();
            var wishlistResponse = await _client.GetAsync(location);
            WishListDetails? wishlist = await wishlistResponse.Parse<WishListDetails>();

            wishlistResponse.EnsureSuccessStatusCode(); // Status Code 200-299
            wishlist.PublicId.Should().NotBeNullOrEmpty();
            wishlist.Description.Should().Be(description);

            Cleanup();
        }

        [Theory]
        [InlineData("description")]
        [InlineData("<b> a description with html</b>")]
        [InlineData(null)]
        public async Task CanCreateAWishlistWithCustomDescriptionAndRetrieveIt(string description)
        {

            (var response, var location) = await CreateWishlist($"{{\"name\":\"My Wishlist\" {(description is null ? "}}" : $",\"description\":\"{description}\"}}")}");
            response.EnsureSuccessStatusCode();
            var wishlistResponse = await _client.GetAsync(location);
            WishListDetails? wishlist = await wishlistResponse.Parse<WishListDetails>();

            wishlistResponse.EnsureSuccessStatusCode(); // Status Code 200-299
            wishlist.PublicId.Should().NotBeNullOrEmpty();
            wishlist.Description.Should().Be(description);

            Cleanup();
        }

        [Fact]
        public async Task CanCreateWishlistAndUpdateDetails()
        {
            var description = "original";
            var expectedDescription = "updated";
            (var response, var location) = await CreateWishlist($"{{\"name\":\"My Wishlist\" {(description is null ? "}}" : $",\"description\":\"{description}\"}}")}");
            var wishlistResponse = await _client.GetAsync(location);
            WishListDetails? wishlist = await wishlistResponse.Parse<WishListDetails>();
            var firstDescription = wishlist.Description;
            var patchResponse = await _client.PatchAsync(location, new StringContent($"{{\"name\":\"My Wishlist\",\"description\":\"{expectedDescription}\"}}", Encoding.UTF8, "application/json"));
            WishListDetails? updatedWishlist = await (await _client.GetAsync(location)).Parse<WishListDetails>();

            patchResponse.EnsureSuccessStatusCode();
            updatedWishlist.Description.Should().Be(expectedDescription);    

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

            (var location, var wishlist, _) = await CreateAndAddItems(itemName);
            wishlist.Items.Should().Contain(i => i.Description == itemName);
            Cleanup();
        }

        [Fact]
        public async Task CanAddItemsWithLinkToWishlist()
        {
            var itemName = "My Wishlist Item";
            var uri = new Uri("https://www.google.com");
            (var location, var wishlist, _) = await CreateAndAddItems(itemName, uri);
            var item = wishlist.Items.First();
            item.Description.Should().Be(itemName);
            item.Link.Should().Be(uri);

            Cleanup();
        }

        [Fact]
        public async Task CanDeleteItem()
        {
            (var location, _, _) = await CreateAndAddItems("My Wishlist Item");

            HttpResponseMessage deleteResponse = await _client.DeleteAsync(location);
            var itemResponse = await _client.GetAsync(location);

            deleteResponse.EnsureSuccessStatusCode();
            itemResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            Cleanup();
        }
    }
}
