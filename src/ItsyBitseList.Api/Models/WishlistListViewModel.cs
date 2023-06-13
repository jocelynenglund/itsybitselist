using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Api.Models
{
    /// <summary>
    /// Request model for wishlist creation
    /// </summary>
    /// <param name="Name"></param>
    public record WishlistCreationRequest(string Name);

    public record ItemCretionRequest(string Details);
}
