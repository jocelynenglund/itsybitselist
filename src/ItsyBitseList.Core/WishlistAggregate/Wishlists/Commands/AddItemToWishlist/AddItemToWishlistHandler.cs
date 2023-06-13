using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.AddItemToWishlist
{

    public record AddItemToWishlistCommand(Guid WishlistId, string ItemDetails): IRequest<Guid>;

    public class AddItemToWishlistHandler : IRequestHandler<AddItemToWishlistCommand, Guid>
    {
        private readonly IAsyncRepository<WishlistItem> _wishlistItemRepository;

        public AddItemToWishlistHandler(IAsyncRepository<WishlistItem> repository)
        {
            _wishlistItemRepository = repository;
        }
        public async Task<Guid> Handle(AddItemToWishlistCommand request, CancellationToken cancellationToken)
        {
            var wishlistItem = WishlistItem.CreateWith(request);
            wishlistItem =  await _wishlistItemRepository.AddAsync(wishlistItem);
            return wishlistItem.Id;
        }
    }
}
