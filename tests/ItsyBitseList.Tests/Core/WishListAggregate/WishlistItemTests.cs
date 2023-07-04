using FluentAssertions;
using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Tests.Core.WishListAggregate
{
    public class WishlistItemTests
    {
        [Fact]
        public void Promised_ShouldReturnId()
        {
            var wishlistItem = new WishlistItem(Guid.NewGuid(), Guid.NewGuid(), "Test");
            var result = wishlistItem.Promised();
            result.Should().NotBeEmpty();
        }

        [Fact]
        public void Revert_ShouldCheckThatIdMatches()
        {
            var wishlistItem = new WishlistItem(Guid.NewGuid(), Guid.NewGuid(), "Test");
            var id = wishlistItem.Promised();
            wishlistItem.Revert(id);
            wishlistItem.State.Should().Be(State.Wished);
        }

        [Fact]
        public void Revert_NotUpdateStateWithMismatchedId()
        {
            var wishlistItem = new WishlistItem(Guid.NewGuid(), Guid.NewGuid(), "Test");
            wishlistItem.Promised();
            FluentActions.Invoking(() => wishlistItem.Revert(Guid.NewGuid())).Should().Throw<InvalidOperationException>();
            wishlistItem.State.Should().Be(State.Promised);
        }
    }
}
