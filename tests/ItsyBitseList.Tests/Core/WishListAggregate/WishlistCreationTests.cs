using FluentAssertions;
using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Tests.Core.WishListAggregate
{
    public class WishlistCreationTests
    {
        // a test to create a new wishlist with a name and owner.
        [Fact]
        public void CreateWishlist()
        {
            // arrange
            const string Name = "My Wishlist";

            // act
            var wishlist = Wishlist.CreateWith(Guid.NewGuid(), name: Name);

            // assert
            Assert.Equal(Name, wishlist.Name);
            Assert.Empty(wishlist.Items);
        }

        // a test adding an item to a wishlist
        [Fact]
        public void AddItemToWishlist()
        {
            // arrange
            var wishlistId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            const string Name = "My Wishlist";
            const string ItemDescription = "My Item";
            var Item = new WishlistItem(itemId, wishlistId, ItemDescription);

            // act
            var wishlist = Wishlist.CreateWith(wishlistId,  name: Name);
            wishlist.AddItem(itemId, ItemDescription);

            // assert
            wishlist.Name.Should().Be(Name);
            wishlist.Items.Should().Contain(item => item.Description == Item.Description);
        }
    }
}
