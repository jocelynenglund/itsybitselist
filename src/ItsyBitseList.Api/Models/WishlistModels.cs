namespace ItsyBitseList.Api.Models
{
    /// <summary>
    /// Request model for wishlist creation
    /// </summary>
    /// <param name="Name"></param>
    public record WishlistCreationRequest(string Name);
    /// <summary>
    /// Request model for item creation
    /// </summary>
    /// <param name="Details"></param>
    /// <param name="Link">Optional link to item</param>
    public record ItemCreationRequest(string Details, Uri? Link);
    
    /// <summary>
    /// Patch request model for item promise
    /// </summary>
    /// <param name="State"></param>
    /// <param name="Id"></param>
    public record PatchRequest(string State, Guid? PromiseKey);
}
