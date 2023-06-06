using FluentAssertions;

namespace ItsyBitseList.IntegrationTests
{
    public class WishlistControllerTests
    {
        [Fact]
        public async Task Test1()
        {
            var client = new ApiClient("https://localhost:7137/", new HttpClient());
            var result = await client.GetWeatherForecastAsync();
            
            result.Should().NotBeNull();

        }


        [Fact]
        public async Task CreateWishlistCollection_RetrievesCollectionWithEmptyList()
        {
            var expectedOwnerName = "Me";
            var client = new ApiClient("https://localhost:7137/", new HttpClient());
            await client.CreateWishlistCollectionAsync(expectedOwnerName);
            var result = await client.GetWishlistCollectionAsync(expectedOwnerName);
            result.Should().BeAssignableTo<IEnumerable<WishlistListViewModel>>();
            result.Count.Should().Be(0);

        }

        [Fact]
        public async Task CreateWishlist_ShouldCreateNewWishlistAndAddToCollection()
        {
            var expectTitle = "My Wishlist";
            var expectedOwner = "Me";
            var client = new ApiClient("https://localhost:7137/", new HttpClient());
            await client.CreateWishlistCollectionAsync(expectedOwner);
            await client.CreateWishlistAsync(expectedOwner, expectTitle);
            var result = await client.GetWishlistCollectionAsync(expectedOwner);

            result.Count.Should().BeGreaterThan(1);
            
        }
    }
}