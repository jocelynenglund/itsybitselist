namespace ItsyBitseList.Core.WishlistCollectionAggregate
{
    // create a Wishlist class according to the tests
    public class Wishlist
    {
        private Wishlist(Guid id, string name)
        {
            Id = id;
            Name = name;
            items = new List<WishlistItem>();
        }
        public Guid Id { get; set; }
        public string Name { get; }
        private List<WishlistItem> items;
        public IReadOnlyCollection<WishlistItem> Items => items.AsReadOnly();

        public static Wishlist CreateWith(Guid id, string name)
        {
            return new Wishlist(id, name);

        }
        // a method to add items to the wishlist
        public void AddItem(Guid id, string item)
        {
            items.Add(new WishlistItem(id, item));
        }
    }
    public record WishlistItem
    {
        public Guid Id { get; } 
        public string Description { get; }
        public WishlistItem(Guid id, string description)
        {
            Id = id;
            Description = description;
        }

    }
}