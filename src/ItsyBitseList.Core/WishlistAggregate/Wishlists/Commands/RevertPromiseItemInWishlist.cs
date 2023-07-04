using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands
{
    public class RevertPromiseItemInWishlist
    {
        public record class RevertPromiseItemInWishlistCommand(Guid WishlistId, Guid ItemId, Guid PromisedBy): IRequest;

        public class RevertPromiseItemInWishlistHandler : IRequestHandler<RevertPromiseItemInWishlistCommand>
        {
            private readonly IAsyncRepository<WishlistItem> _repository;
            public RevertPromiseItemInWishlistHandler(IAsyncRepository<WishlistItem> repository)
            {
                _repository = repository;
            }
            public async Task Handle(RevertPromiseItemInWishlistCommand request, CancellationToken cancellationToken)
            {
                var toRevert = await _repository.GetByIdAsync(request.ItemId);
                if (toRevert.WishlistId != request.WishlistId)
                {
                    throw new UnauthorizedAccessException("Item not found in wishlist");
                }
                else
                {
                    toRevert.Revert(request.PromisedBy);
                    await _repository.UpdateAsync(toRevert);
                }
            }
        }
    }
}
