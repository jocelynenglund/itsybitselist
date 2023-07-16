using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.CompilerServices;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.AddItemToWishlist;

[assembly:InternalsVisibleTo("ItsyBitseList.IntegrationTests")]
[assembly:InternalsVisibleTo("ItsyBitseList.Tests")]
namespace ItsyBitseList.Core.WishlistCollectionAggregate
{
    public record WishlistItemState(Guid Id, Guid WishlistId, [JsonConverter(typeof(StringEnumConverter))] State State, Guid? PromiseGuid, string Description, Uri? Link);
    public record WishlistItem
    {
        public WishlistItemState DataState => new WishlistItemState(Id, WishlistId, State, PromiseGuid, Description, Link);
        public Guid Id { get; init; }
        public string Description { get; }
        public Uri? Link { get; init; }
        public State State { get; private set; }
        public Guid WishlistId { get; set; }
        public Guid? PromiseGuid {  get; private set; }
        
        internal WishlistItem(Guid id, Guid wishlistId, string description, Uri? link=null)
        {
            Id = id;
            Description = description;
            State = State.Wished;
            WishlistId = wishlistId;
            Link = link;
        }
        private WishlistItem(Guid id, Guid wishlistId, string description, State state, Guid? promiseGuid, Uri? link=null) : this(id, wishlistId, description, link)
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
        internal static WishlistItem CreateWith(WishlistItemState x)
        {
            return new WishlistItem(x.Id, x.WishlistId, x.Description, x.State, x.PromiseGuid, x.Link);
        }
        internal static WishlistItem CreateWith(AddItemToWishlistCommand request)
        {
            return new WishlistItem(Guid.NewGuid(), request.WishlistId, request.ItemDetails);
        }
    }
}