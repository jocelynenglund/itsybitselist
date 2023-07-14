using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using Moq;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.PromiseItemInWishlist;

namespace ItsyBitseList.Tests.Core.WishListAggregate.Wishlists.Commands
{
    public class PromiseItemInWishlistTests
    {
        private readonly Mock<IAsyncRepository<Wishlist>> _wishlistRepositoryMock;
        private readonly PromiseItemInWishlistHandler _sut;

        public PromiseItemInWishlistTests()
        {
            _wishlistRepositoryMock = new();
            _sut = new PromiseItemInWishlistHandler(_wishlistRepositoryMock.Object);
        }

        //[Fact]
        //public async Task PromiseItemShouldReturnAnId()
        //{
        //    var wishlistItem = new WishlistItem(Guid.NewGuid(), Guid.NewGuid(), "Test");
        //    _wishlistRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(wishlistItem);

        //    var result = await _sut.Handle(new PromiseItemInWishlistCommand(wishlistItem.WishlistId, wishlistItem.Id), CancellationToken.None);

        //    wishlistItem.State.Should().Be(State.Promised);
        //    result.Should().NotBeEmpty();
        //}       
    }
}
