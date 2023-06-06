using FluentAssertions;
using ItsyBitseList.Api.Controllers;
using ItsyBitseList.Api.Models;
using ItsyBitseList.Core.Interfaces;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using Moq;

namespace ItsyBitseList.Tests.Api.Controllers
{
    public class WishlistCollectionControllerTests
    {
        private readonly Mock<IWishlistCollectionRepository> _repositoryMock;
        private readonly WishlistCollectionController _sut;

        public WishlistCollectionControllerTests()
        {
            _repositoryMock = new Mock<IWishlistCollectionRepository>();
            _sut = new WishlistCollectionController(_repositoryMock.Object);
            
        }
        [Fact]
        public void Get_ShouldReturnAListOfOwnedWishlists()
        {
            // Arrange
            var wishlistCollection = new WishlistCollection("Celyn");
            wishlistCollection.Wishlists.Add(Wishlist.CreateWith("My Wishlist"));
            _repositoryMock.Setup(x => x.GetWishlistCollectionByOwner(It.IsAny<string>()))
                .Returns(wishlistCollection);
            
            // Act
            var response = _sut.Get("Celyn");
            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeAssignableTo<IEnumerable<WishlistListViewModel>>();
            response.Value.Count().Should().Be(1);
        }

        [Fact]
        public void CreateWishlistCollection_ShouldCreateANewWishlistCollection()
        {
            _sut.Post(new WishlistCollectionCreationRequest("Celyn"));

            _repositoryMock.Verify(x => x.CreateWishlistCollection(It.IsAny<string>()), Times.Once);
        }
    }
}
