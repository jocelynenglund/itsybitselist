using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands
{
    public class DeleteItemInWishlist
    {
        public record DeleteItemInWishlistCommand(
            Guid WishlistId,
            Guid ItemId
        ) : IRequest;

        public class DeleteItemInWishlistHandler : IRequestHandler<DeleteItemInWishlistCommand>
        {
            public IAsyncRepository<Wishlist> _repository;
            public DeleteItemInWishlistHandler(IAsyncRepository<Wishlist> repository)
            {
                _repository = repository;
            }
            public async Task Handle(DeleteItemInWishlistCommand request, CancellationToken cancellationToken)
            {
                var wishlist = await _repository.GetByIdAsync(request.WishlistId);
                var itemToDelete = wishlist.Items.First(item => item.Id == request.ItemId);
                if (itemToDelete is null || itemToDelete.WishlistId != request.WishlistId)
                {
                    throw new UnauthorizedAccessException("Item not found in wishlist");
                }
                wishlist.Remove(request.ItemId);
                await _repository.UpdateAsync(wishlist);
            }

        }
    }
}
