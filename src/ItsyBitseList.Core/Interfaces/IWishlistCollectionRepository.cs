using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Core.Interfaces
{
    public interface IWishlistCollectionRepository
    {
        void CreateWishlistCollection(string owner, Guid id, string wishlistName);
        WishlistCollection? GetWishlistCollectionByOwner(string owner);
    }
}
