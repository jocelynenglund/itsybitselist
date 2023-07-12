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
            private readonly IAsyncRepository<Wishlist> _repository;
            public RevertPromiseItemInWishlistHandler(IAsyncRepository<Wishlist> repository)
            {
                _repository = repository;
            }
            public async Task Handle(RevertPromiseItemInWishlistCommand request, CancellationToken cancellationToken)
            {
                var wishlist = await _repository.GetByIdAsync(request.WishlistId);

                var toRevert = wishlist.Items.First(i => i.Id == request.ItemId);
                if (toRevert.WishlistId != request.WishlistId)
                {
                    throw new UnauthorizedAccessException("Item not found in wishlist");
                }
                else
                {
                    toRevert.Revert(request.PromisedBy);
                    await _repository.UpdateAsync(wishlist);
                }
            }
        }
    }
}
