using ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.AddItemToWishlist;

namespace ItsyBitseList.Core.WishlistCollectionAggregate
{
    public record WishlistItem
    {
        public Guid Id { get; init; }
        public string Description { get; }
        public State State { get; private set; }
        public Guid WishlistId { get; set; }
        public WishlistItem(Guid id, Guid wishlistId, string description)
        {
            Id = id;
            Description = description;
            State = State.Wished;
            WishlistId = wishlistId;
        }
        public void Promised()
        {
            State = State.Promised;
        }
        public void Verified()
        {
            State = State.Verified;
        }

        internal static WishlistItem CreateWith(AddItemToWishlistCommand request)
        {
            return new WishlistItem(Guid.NewGuid(), request.WishlistId, request.ItemDetails);
        }
    }
}