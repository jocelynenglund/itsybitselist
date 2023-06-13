using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist
{
    public record ItemDetails(Guid Id, State State, string Description);
    public record GetItemInWishlistQuery(Guid WishlistId, Guid ItemId): IRequest<ItemDetails>;
    public class GetItemInWishlistHandler : IRequestHandler<GetItemInWishlistQuery, ItemDetails>
    {
        private readonly IAsyncRepository<WishlistItem> _repository;
        public GetItemInWishlistHandler(IAsyncRepository<WishlistItem> repository)
        {
            _repository = repository;
        }
        public async Task<ItemDetails> Handle(GetItemInWishlistQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetByIdAsync(request.ItemId);
            if (result.WishlistId == request.WishlistId)
            {
                return result.AsItemDetails();
            } else
            {
                throw new UnauthorizedAccessException("Item not found in wishlist");
            }

        }
    }
}
