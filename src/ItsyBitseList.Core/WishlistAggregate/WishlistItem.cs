using ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.AddItemToWishlist;
using System.Text.Json.Serialization;

namespace ItsyBitseList.Core.WishlistCollectionAggregate
{
    public record WishlistItem
    {
        public Guid Id { get; init; }
        public string Description { get; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public State State { get; private set; }
        public Guid WishlistId { get; set; }

        [JsonInclude]
        public Guid? PromiseGuid {  get; private set; }
        
        public WishlistItem(Guid id, Guid wishlistId, string description)
        {
            Id = id;
            Description = description;
            State = State.Wished;
            WishlistId = wishlistId;
        }
        [JsonConstructor]
        public WishlistItem(Guid id, Guid wishlistId, string description, State state, Guid? promiseGuid) : this(id, wishlistId, description)
        {
            State = state;
            PromiseGuid = promiseGuid;
        }

        public Guid Promised()
        {
            State = State.Promised;
            PromiseGuid = Guid.NewGuid();
            return PromiseGuid.Value;
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
            if (PromiseGuid == id)
            {
                State = State.Wished;
            } else
            {
                throw new InvalidOperationException("Cannot revert promise with mismatched id");
            }
        }
    }
}