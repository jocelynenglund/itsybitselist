namespace ItsyBitseList.Core.WishlistCollectionAggregate
{
    // create a Wishlist class according to the tests
    public class Wishlist
    {
        private Wishlist(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            items = new List<WishlistItem>();
        }
        public Guid Id { get; set; }
        public string Name { get; }
        private List<WishlistItem> items;
        public IReadOnlyCollection<WishlistItem> Items => items.AsReadOnly();

        public static Wishlist CreateWith(string name)
        {
            return new Wishlist(name);

        }
        // a method to add items to the wishlist
        public void AddItem(string item)
        {
            items.Add(new WishlistItem(item));
        }
    }
    public record WishlistItem
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Description { get; }
        public WishlistItem(string description)
        {
            Description = description;
        }

    }
}