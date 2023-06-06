namespace ItsyBitseList.Api.Models
{
    public record WishlistListViewModel(Guid id, string Title, int numberOfItems);
    public record WishlistDetailsViewModel(string Name, IEnumerable<Item> Items);
    public record Item(string Details);

    public record WishlistCollectionCreationRequest(string Owner);
    public record WishlistCreationRequest(string Name);
}
