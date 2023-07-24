using ItsyBitseList.Core.Interfaces;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.CreateWishlist;

namespace ItsyBitseList.Core.WishlistCollectionAggregate
{
    public record WishlistSettings(string Name, string? Description);
    public record WishlistState(Guid Id, string Name, string Owner, List<WishlistItemState> Items, string? Description);
    // create a Wishlist class according to the tests
    public class Wishlist : IRootEntity
    {
        public WishlistState DataState => new WishlistState(Id, Name, Owner, Items.Select(x => x.DataState).ToList(), Description);
        public static string DefaultOwner = "Anonymous";
        
        private Wishlist(Guid id, string name, string owner, string? description=null)
        {
            Id = id;
            Name = name;
            Owner = owner;
            Description = description;
        }
        public Guid Id { get; init; }
        public string Name { get; private set; }
        public string Owner { get; init; } = DefaultOwner;
        public string? Description { get; private set; }
        private List<WishlistItem> items = new();

        public void SetItems(List<WishlistItem> items) => this.items = items;
        public IReadOnlyCollection<WishlistItem> Items { get => items.AsReadOnly(); init => items = value.ToList(); }

        public static Wishlist CreateWith(Guid id, string name, string? owner = null)
        {
            return new Wishlist(id, name, owner ?? Guid.NewGuid().ToString());
        }
        public static Wishlist CreateWith(WishlistState state)
        {
            return new Wishlist(state.Id, state.Name, state.Owner, state.Description)
            {
                items = state.Items.Select(x => WishlistItem.CreateWith(x)).ToList()
            };
        }

        // a method to add items to the wishlist
        public Wishlist AddItem(Guid id, string item, Uri? link = null)
        {
            items.Add(new WishlistItem(id, this.Id, item, link));
            return this;
        }

        public void Remove(Guid id)
        {
            items.Remove(items.First(x => x.Id == id));
        }

        internal static Wishlist CreateWith(CreateWishlistCommand request)
        {
            return new Wishlist(Guid.NewGuid(), request.WishlistName, request.Owner ?? DefaultOwner, request.Description);
        }

        internal void UpdateSettings(WishlistSettings settings)
        {
            Name = settings.Name;
            Description = settings.Description;

        }
    }
}