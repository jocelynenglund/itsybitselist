using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.PromiseItemInWishlist
{

    public record class PromiseItemInWishlistCommand(Guid WishlistId, Guid ItemId): IRequest;
    public class PromiseItemInWishlistHandler : IRequestHandler<PromiseItemInWishlistCommand>
    {
        private readonly IAsyncRepository<WishlistItem> _repository;
        public PromiseItemInWishlistHandler(IAsyncRepository<WishlistItem> repository)
        {

            _repository = repository;

        }
        public async Task Handle(PromiseItemInWishlistCommand request, CancellationToken cancellationToken)
        {
            var toPromise = await _repository.GetByIdAsync(request.ItemId);
            if (toPromise.WishlistId != request.WishlistId)
            {
                throw new UnauthorizedAccessException("Item not found in wishlist");
            }
            else
            {
                toPromise.Promised();
                await _repository.UpdateAsync(toPromise);
            }
        }
    }
}
