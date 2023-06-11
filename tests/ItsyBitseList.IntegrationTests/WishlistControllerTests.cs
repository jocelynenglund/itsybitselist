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
        public async Task CreateWishlistCollection_RetrievesCollectionWithListOfOne()
        {
            var expectTitle = "My Wishlist";
            var expectedOwner = "Me";
            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("owner", expectedOwner);
            var client = new ApiClient("https://localhost:7137/", httpClient);

            await client.WishlistCollectionAsync(expectedOwner, new WishlistCollectionCreationRequest() { WishlistName = expectedOwner });

            var result = await client.WishlistCollectionAllAsync(expectedOwner);
            result.Count.Should().BeGreaterThanOrEqualTo(1);

        }

    }
}