using MediatR;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist.GetItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;

namespace ItsyBitseList.Core.Interfaces.App
{
    public record WishlistCreationRequest(string? Owner, string Name);
    public record ItemCreationRequest(string Details, Uri? Link);
    /// <summary>
    /// Patch request model for item promise
    /// </summary>
    /// <param name="State"></param>
    /// <param name="Id"></param>
    public record PatchRequest(string State, Guid? PromiseKey);
    public interface IWishlistApp
    {
        Task<Response<Guid>> AddItemToWishlist(Guid id, ItemCreationRequest request);
        Task<Response<Guid>> CreateWishlist(WishlistCreationRequest request);
        Task<Response<Unit>> DeleteWishlist(Guid id, string? owner = null);
        Task<Response<Unit>> DeleteWishlistItem(Guid wishlistId, Guid itemId);
        Task<Response<ItemDetails>> GetItemInWishlist(string wishlistId, Guid itemId);
        Task<Response<WishListDetails>> GetWishlist(string id);
        Task<Response<Guid>> PromiseItem(string wishlistId, Guid itemId);
        Task<Response<Guid>> RevertPromise(string wishlistId, Guid itemId, Guid promiseKey);
    }
}
