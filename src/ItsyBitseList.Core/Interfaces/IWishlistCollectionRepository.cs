using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Core.Interfaces
{
    public interface IWishlistCollectionRepository
    {
        void CreateWishlistCollection(string owner, string wishlistName);
        WishlistCollection GetWishlistCollectionByOwner(string owner);
    }
}
