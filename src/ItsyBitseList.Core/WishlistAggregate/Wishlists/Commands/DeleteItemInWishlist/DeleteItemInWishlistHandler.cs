using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.DeleteItemInWishlist
{
    public record DeleteItemInWishlistCommand(
        Guid WishlistId,
        Guid ItemId
    ) : IRequest;

    public class DeleteItemInWishlistHandler : IRequestHandler<DeleteItemInWishlistCommand>
    {
        public IAsyncRepository<WishlistItem> _wishlistItemRepository;
        public DeleteItemInWishlistHandler(IAsyncRepository<WishlistItem> repository)
        {
            _wishlistItemRepository = repository;
        }
        public async Task Handle(DeleteItemInWishlistCommand request, CancellationToken cancellationToken)
        {
            var itemToDelete = await _wishlistItemRepository.GetByIdAsync(request.ItemId);
            if (itemToDelete is null)
            {
                throw new UnauthorizedAccessException("Item not found in wishlist");
            }
            else if (itemToDelete.WishlistId != request.WishlistId)
            {
                throw new UnauthorizedAccessException("Item not found in wishlist");
            }
            await _wishlistItemRepository.DeleteAsync(itemToDelete);
        }

    }
}
