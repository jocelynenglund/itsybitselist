using ItsyBitseList.Core.Interfaces;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using System.ComponentModel;

namespace ItsyBitseList.Infrastructure.Data
{
    public class InMemoryRepository : IWishlistCollectionRepository
    {
        List<WishlistCollection> wishlistCollections;
        public IEnumerable<WishlistCollection> WishlistCollections => wishlistCollections;
        public InMemoryRepository(bool seeded = true)
        {
            wishlistCollections = new List<WishlistCollection>();
            if (seeded) SeedData();
        }

        private void SeedData()
        {
            var me = new WishlistCollection("me");

            me.CreateNewWishlist(Guid.NewGuid(), "Birthday Wishlist");
            me.CreateNewWishlist(Guid.NewGuid(), "Christmas Wishlist");
            wishlistCollections.Add(me);
            me.Wishlists[0].AddItem(Guid.NewGuid(), "Bio kort");
            me.Wishlists[0].AddItem(Guid.NewGuid(), "Barbie Doll");
            me.Wishlists[1].AddItem(Guid.NewGuid(), "Robux");
            me.Wishlists[1].AddItem(Guid.NewGuid(), "Christmas dress");
        }

        public WishlistCollection GetWishlistCollectionByOwner(string owner)
        {
            return WishlistCollections.First(collection => collection.Owner == owner);
        }

        public void CreateWishlistCollection(string owner, Guid id, string wishlistName)
        {
            if (!wishlistCollections.Any(item => item.Owner == owner))
            {
                var collection = new WishlistCollection(owner);
                collection.CreateNewWishlist(id, wishlistName);
                wishlistCollections.Add(collection);
            }
        }
    }
}
