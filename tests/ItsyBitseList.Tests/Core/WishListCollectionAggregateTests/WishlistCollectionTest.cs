using FluentAssertions;
using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Tests.Core.WishListCollectionAggregateTests
{
    public class WishlistCollectionTest
    {
        [Fact]
        public void Initializer_WishlistCollection_Empty()
        {
            //Arrange
            var expectedItemCount = 0;
            var expectedOwner = "Celyn";

            //Act
            var wishlist = new WishlistCollection(expectedOwner);

            //Assert
            Assert.NotNull(wishlist);
            Assert.IsType<WishlistCollection>(wishlist);
            Assert.Equal(expectedItemCount, wishlist.Wishlists.Count);
            wishlist.Owner.Should().Be(expectedOwner);
        }

        [Fact]
        public void WishlistCollection_CreateNewWishlist_SetsTitleAndDescription()
        {
            //Arrange
            var wishlistCollection = new WishlistCollection("Celyn");

            //Act
            wishlistCollection.CreateNewWishlist("My Wishlist");

            //Assert
            wishlistCollection.Wishlists.Should().HaveCount(1);
            wishlistCollection.Wishlists[0].Name.Should().Be("My Wishlist");
        }
    }
}
