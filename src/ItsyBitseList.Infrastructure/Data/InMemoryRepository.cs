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
            var me  = new WishlistCollection("me");
            
            me.CreateNewWishlist("Birthday Wishlist");
            me.CreateNewWishlist("Christmas Wishlist");
            wishlistCollections.Add(me);
            me.Wishlists[0].AddItem("Bio kort");
            me.Wishlists[0].AddItem("Barbie Doll");
            me.Wishlists[1].AddItem("Robux");
            me.Wishlists[1].AddItem("Christmas dress");
        }

        public WishlistCollection GetWishlistCollectionByOwner(string owner)
        {
            return WishlistCollections.First(collection => collection.Owner == owner);
        }

        public void CreateWishlistCollection(string owner, string wishlistName)
        {
            if (!wishlistCollections.Any(item => item.Owner == owner))
            {
                var collection = new WishlistCollection(owner);
                collection.CreateNewWishlist(wishlistName);
                wishlistCollections.Add(collection);
            }
        }
    }
}
