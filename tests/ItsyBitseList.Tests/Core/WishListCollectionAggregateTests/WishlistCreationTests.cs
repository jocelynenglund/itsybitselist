using FluentAssertions;
using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Tests.Core.WishListCollectionAggregateTests
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
            var wishlist = Wishlist.CreateWith(name: Name);

            // assert
            Assert.Equal(Name, wishlist.Name);
            Assert.Empty(wishlist.Items);
        }

        // a test adding an item to a wishlist
        [Fact]
        public void AddItemToWishlist()
        {
            // arrange
            const string Name = "My Wishlist";
            const string ItemDescription = "My Item";
            var Item = new WishlistItem(ItemDescription);

            // act
            var wishlist = Wishlist.CreateWith(name: Name);
            wishlist.AddItem(ItemDescription);

            // assert
           wishlist.Name.Should().Be(Name);
            wishlist.Items.Should().Contain(item => item.Description == Item.Description);
        }
    }
}
