using FluentAssertions;
using ItsyBitseList.Infrastructure.Persistence;

namespace ItsyBitseList.Tests.Infrastructure.Persitence
{
    public class InMemoryRepositoryTests
    {
        [Fact]
        public void WishlistCollection_ListShouldBeInitializedOnCreation()
        {
            // Arrange
            var repository = new InMemoryRepository(seeded: false);

            // Act
            var result = repository.Wishhlists;

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
