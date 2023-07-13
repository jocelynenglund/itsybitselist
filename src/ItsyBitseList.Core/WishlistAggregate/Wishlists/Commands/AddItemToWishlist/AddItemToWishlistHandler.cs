using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.AddItemToWishlist
{

    public record AddItemToWishlistCommand(Guid WishlistId, string ItemDetails, Uri? Link = null): IRequest<Guid>;

    public class AddItemToWishlistHandler : IRequestHandler<AddItemToWishlistCommand, Guid>
    {
        private readonly IAsyncRepository<Wishlist> _wishlistRepository;

        public AddItemToWishlistHandler(IAsyncRepository<Wishlist> repository)
        {
            _wishlistRepository = repository;
        }
        public async Task<Guid> Handle(AddItemToWishlistCommand request, CancellationToken cancellationToken)
        {
            //var wishlistItem = WishlistItem.CreateWith(request);
            var wishlist = await _wishlistRepository.GetByIdAsync(request.WishlistId);
            var itemGuid = Guid.NewGuid();
            wishlist.AddItem(itemGuid, request.ItemDetails, request.Link);
            await _wishlistRepository.UpdateAsync(wishlist);
            return itemGuid;
        }
    }
}
