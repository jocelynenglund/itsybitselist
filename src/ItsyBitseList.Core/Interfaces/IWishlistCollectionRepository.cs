using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Core.Interfaces
{
    public interface IWishlistCollectionRepository
    {
        void CreateWishlistCollection(string v);
        WishlistCollection GetWishlistCollectionByOwner(string owner);
    }
}
