
using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Core.Interfaces.Persistence
{
    public interface IWishlistRepository : IAsyncRepository<Wishlist>
    {
        Task<IEnumerable<Wishlist>> GetWishlistByOwnerAsync(string owner);
    }
}
