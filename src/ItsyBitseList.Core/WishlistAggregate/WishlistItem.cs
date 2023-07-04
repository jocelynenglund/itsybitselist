using ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.AddItemToWishlist;

namespace ItsyBitseList.Core.WishlistCollectionAggregate
{
    public record WishlistItem
    {
        public Guid Id { get; init; }
        public string Description { get; }
        public State State { get; private set; }
        public Guid WishlistId { get; set; }

        private Guid _promiseGuid;
        
        public WishlistItem(Guid id, Guid wishlistId, string description)
        {
            Id = id;
            Description = description;
            State = State.Wished;
            WishlistId = wishlistId;
        }
        public Guid Promised()
        {
            State = State.Promised;
            _promiseGuid = Guid.NewGuid();
            return _promiseGuid;
        }
        public void Verified()
        {
            State = State.Verified;
        }

        internal static WishlistItem CreateWith(AddItemToWishlistCommand request)
        {
            return new WishlistItem(Guid.NewGuid(), request.WishlistId, request.ItemDetails);
        }

        public void Revert(Guid id)
        {
            if (_promiseGuid == id)
            {
                State = State.Wished;
            } else
            {
                throw new InvalidOperationException("Cannot revert promise with mismatched id");
            }
        }
    }
}