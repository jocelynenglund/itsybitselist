using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist
{
    public record ItemDetails(Guid Id, State State, string Description);
    public record GetItemInWishlistQuery(Guid WishlistId, Guid ItemId): IRequest<ItemDetails>;
    public class GetItemInWishlistHandler : IRequestHandler<GetItemInWishlistQuery, ItemDetails>
    {
        private readonly IAsyncRepository<Wishlist> _repository;
        public GetItemInWishlistHandler(IAsyncRepository<Wishlist> repository)
        {
            _repository = repository;
        }
        public async Task<ItemDetails> Handle(GetItemInWishlistQuery request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetByIdAsync(request.WishlistId)).Items.First(i =>i.Id == request.ItemId);

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
