using FluentAssertions;
using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.PromiseItemInWishlist;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItsyBitseList.Tests.Core.WishListAggregate.Wishlists.Commands
{
    public class PromiseItemInWishlistTests
    {
        private readonly Mock<IAsyncRepository<WishlistItem>> _wishlistRepositoryMock;
        private readonly PromiseItemInWishlistHandler _sut;

        public PromiseItemInWishlistTests()
        {
            _wishlistRepositoryMock = new();
            _sut = new PromiseItemInWishlistHandler(_wishlistRepositoryMock.Object);
        }

        [Fact]
        public async Task PromiseItemShouldReturnAnId()
        {
            var wishlistItem = new WishlistItem(Guid.NewGuid(), Guid.NewGuid(), "Test");
            _wishlistRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(wishlistItem);

            var result = await _sut.Handle(new PromiseItemInWishlistCommand(wishlistItem.WishlistId, wishlistItem.Id), CancellationToken.None);

            wishlistItem.State.Should().Be(State.Promised);
            result.Should().NotBeEmpty();
        }       
    }
}
