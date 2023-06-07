namespace ItsyBitseList.Api.Models
{
    /// <summary>
    /// Summary of a wishlist collection
    /// </summary>
    /// <param name="id">Guid to uniquely identify wishlist collection</param>
    /// <param name="title">Title</param>
    /// <param name="numberOfItems">Number of items in the wishlist</param>
    public record WishlistOverview(Guid id, string title, int numberOfItems);

    /// <summary>
    /// Detail view of wishlist
    /// </summary>
    /// <param name="name">Name of the wishlist e.g. 'Christmas list' or 'Birthday list'</param>
    /// <param name="items">List of items in the wishlist</param>
    public record WishlistDetails(string name, IEnumerable<Item> items);

    /// <summary>
    /// Item details in a wishlist
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Details"></param>
    public record Item(Guid Id, string Details);

    /// <summary>
    /// Request model for wishlist collection creation
    /// </summary>
    /// <param name="Owner"></param>
    public record WishlistCollectionCreationRequest(string WishlistName);

    /// <summary>
    /// Request model for wishlist creation
    /// </summary>
    /// <param name="Name"></param>
    public record WishlistCreationRequest(string Name);

    public record ItemCretionRequest(string Details);
}
