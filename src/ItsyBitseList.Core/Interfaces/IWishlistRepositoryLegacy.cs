using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Core.Interfaces
{
    public interface IWishlistRepositoryLegacy
    {
        void CreateWishlist(string owner, Guid id, string wishlistName);
        IEnumerable<Wishlist> GetWishlistCollectionByOwner(string owner);
        Wishlist GetWishlist(Guid id);
    }
}
