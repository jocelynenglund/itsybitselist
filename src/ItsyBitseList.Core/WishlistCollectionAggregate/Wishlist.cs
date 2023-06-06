namespace ItsyBitseList.Core.WishlistCollectionAggregate
{
    // create a Wishlist class according to the tests
    public class Wishlist
    {
        private Wishlist(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            items = new List<string>();
        }
        public Guid Id { get; set; }
        public string Name { get; }
        private List<string> items;
        public IReadOnlyCollection<string> Items => items.AsReadOnly();

        public static Wishlist CreateWith(string name)
        {
            return new Wishlist(name);

        }
        // a method to add items to the wishlist
        public void AddItem(string item)
        {
            items.Add(item);
        }
    }
}