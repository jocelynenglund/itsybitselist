using ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.CreateWishlist;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;

namespace ItsyBitseList.Core.WishlistCollectionAggregate
{
    // create a Wishlist class according to the tests
    public class Wishlist
    {
        private Wishlist(Guid id, string name, string owner)
        {
            Id = id;
            Name = name;
            items = items ?? new List<WishlistItem>();
            Owner = owner;
        }
        public Guid Id { get; set; }
        public string Name { get; }
        public string Owner { get; set; }
        private List<WishlistItem> items;

        public void SetItems(List<WishlistItem> items) => this.items = items;
        public IReadOnlyCollection<WishlistItem> Items => items.AsReadOnly();

        public static Wishlist CreateWith(Guid id, string name, string? owner = null)
        {
            return new Wishlist(id, name, owner ?? Guid.NewGuid().ToString());
        }

        // a method to add items to the wishlist
        public Wishlist AddItem(Guid id, string item)
        {
            items.Add(new WishlistItem(id, this.Id, item));
            return this;
        }

        public void Remove(Guid id)
        {
            items.Remove(items.First(x => x.Id == id));
        }

        internal static Wishlist CreateWith(CreateWishlistCommand request)
        {
            return new Wishlist(Guid.NewGuid(), request.WishlistName, request.Owner);
        }
    }
}