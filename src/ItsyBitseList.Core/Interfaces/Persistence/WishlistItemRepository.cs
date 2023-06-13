using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Core.Interfaces.Persistence
{
    public interface WishlistItemRepository : IAsyncRepository<WishlistItem>
    {
        Task<IReadOnlyList<WishlistItem>> GetWishlistItemsByWishlistIdAsync(Guid wishlistId);
    }
}
