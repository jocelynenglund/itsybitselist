namespace ItsyBitseList.Tests
{
    public class WishlistCreationTests
    {
        // a test to create a new wishlist with a name and owner.
        [Fact]
        public void CreateWishlist()
        {
            // arrange
            var wishlist = new Wishlist("My Wishlist", "John Doe");

            // act

            // assert
            Assert.Equal("My Wishlist", wishlist.Name);
            Assert.Equal("John Doe", wishlist.Owner);
            Assert.Empty(wishlist.Items);
        }
    }
}
