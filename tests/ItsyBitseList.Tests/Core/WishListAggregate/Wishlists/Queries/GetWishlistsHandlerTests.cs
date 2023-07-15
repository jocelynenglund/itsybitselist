using FluentAssertions;
using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Tests.Mocks;
using Moq;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlists;

namespace ItsyBitseList.Tests.Core.WishListAggregate.Wishlists.Queries
{
    public class GetWishlistsHandlerTests
    {
        private readonly Mock<IWishlistRepository> _wishlistRepositoryMock;
        private readonly GetWishlistsHandler _sut;

        public GetWishlistsHandlerTests()
        {
            _wishlistRepositoryMock = RepositoryMocks.GetWishlistRepositoryMock();

            _sut = new GetWishlistsHandler(_wishlistRepositoryMock.Object);
        }

        [Fact]
        public async Task GetWishlistsTest()
        {
            var result = await _sut.Handle(new GetWishlistsQuery(RepositoryMocks.Owner), CancellationToken.None);

            result.First(r => r.Id == RepositoryMocks.FirstWishlist).ItemCount.Should().Be(2);
            result.First(r => r.Id == RepositoryMocks.SecondWishlist).ItemCount.Should().Be(0);

        }


    }
}
