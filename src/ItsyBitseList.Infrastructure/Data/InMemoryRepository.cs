using ItsyBitseList.Core.Interfaces;
using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Infrastructure.Data
{
    public class InMemoryRepository : IWishlistCollectionRepository
    {
        List<WishlistCollection> wishlistCollections;
        public IEnumerable<WishlistCollection> WishlistCollections => wishlistCollections;
        public InMemoryRepository()
        {
            wishlistCollections = new List<WishlistCollection>();
        }
        public WishlistCollection GetWishlistCollectionByOwner(string owner)
        {
            return WishlistCollections.First(collection => collection.Owner == owner);
        }

        public void CreateWishlistCollection(string owner)
        {
            if (!wishlistCollections.Any(item => item.Owner == owner))
                wishlistCollections.Add(new WishlistCollection(owner));
        }
    }
}
