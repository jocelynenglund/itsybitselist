using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItsyBitseList.Tests.Mocks
{
    internal class RepositoryMocks
    {
        public static Guid FirstWishlist = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
        public static Guid SecondWishlist = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}");
        public static Guid ThirdWishlist = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}");

        public static Guid Item1 = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
        public static Guid Item2 = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}");
        public static string Owner = "TestOwner";
        public static Mock<IWishlistRepository> GetWishlistRepositoryMock()
        {
            var wishlistRepositoryMock = new Mock<IWishlistRepository>();
            wishlistRepositoryMock.Setup(repo => repo.ListAllAsync())
                .ReturnsAsync(GetTestWishlists());
            wishlistRepositoryMock.Setup(repo => repo.GetWishlistByOwnerAsync(It.IsAny<string>()))
               .ReturnsAsync((string owner) => GetTestWishlists().Where(w => w.Owner == owner).ToList());
            wishlistRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Wishlist>()))
                .ReturnsAsync((Wishlist wishlist) =>
                {
                    wishlistRepositoryMock.Setup(repo => repo.ListAllAsync())
                        .ReturnsAsync(GetTestWishlists().Append(wishlist).ToList());
                    return wishlist;
                });
            return wishlistRepositoryMock;
        }

        private static IReadOnlyList<Wishlist> GetTestWishlists()
        {
            var wishlistsCollection = new List<Wishlist>();

            wishlistsCollection.Add(Wishlist.CreateWith(FirstWishlist, "My First Wishlist", Owner).AddItem(Item1, "first item").AddItem(Item2, "secondItem"));
            wishlistsCollection.Add(Wishlist.CreateWith(SecondWishlist, "My Second Wishlist", Owner));
            wishlistsCollection.Add(Wishlist.CreateWith(ThirdWishlist, "My Third Wishlist", "Another Owner"));

            return wishlistsCollection.AsReadOnly();
        }


    }
}
