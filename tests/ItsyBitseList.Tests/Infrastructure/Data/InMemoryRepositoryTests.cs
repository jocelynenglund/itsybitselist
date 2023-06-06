using FluentAssertions;
using ItsyBitseList.Infrastructure.Data;

namespace ItsyBitseList.Tests.Infrastructure.Data
{
    public class InMemoryRepositoryTests
    {
        [Fact]
        public void WishlistCollection_ListShouldBeInitializedOnCreation()
        {
            // Arrange
            var repository = new InMemoryRepository();

            // Act
            var result = repository.WishlistCollections;

            // Assert
            Assert.NotNull(result);
            result.Count().Should().Be(0);
        }

        //[Fact]
        //public void WishlistCollection_CanAddWishlistAndRetrieveByOwnerName()
        //{
        //    // Arrange
        //    var repository = new InMemoryRepository();

        //    // Act
        //    var result = repository.

        //    // Assert
        //    Assert.NotNull(result);
        //    result.Count().Should().Be(0);
        //}
    }
}
