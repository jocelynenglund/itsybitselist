using ItsyBIT.Utilities;
using ItsyBitseList.Core.Constants;
using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist
{
    public class GetItemInWishlist
    {
        public record ItemDetails(Guid Id, State State, string Description);
        public record GetItemInWishlistQuery(Guid WishlistId, Guid ItemId) : IRequest<ItemDetails>
        {
            public GetItemInWishlistQuery(string publicId, Guid itemId) :
                this(
                    Guid.TryParse(publicId, out var parsedId)
                        ? parsedId
                        : new EncodedIdentifier(publicId).Guid,
                    itemId)
            { }
        }

        public class GetItemInWishlistHandler : IRequestHandler<GetItemInWishlistQuery, ItemDetails>
        {
            private readonly IAsyncRepository<Wishlist> _repository;
            public GetItemInWishlistHandler(IAsyncRepository<Wishlist> repository)
            {
                _repository = repository;
            }
            public async Task<ItemDetails> Handle(GetItemInWishlistQuery request, CancellationToken cancellationToken)
            {
                var result = (await _repository.GetByIdAsync(request.WishlistId)).Items.FirstOrDefault(i => i.Id == request.ItemId);

                if (result == null || result.WishlistId != request.WishlistId)
                {
                    throw new InvalidOperationException(ErrorMessages.ItemNotFound);
                }
                else
                {
                    return result.AsItemDetails();
                }

            }
        }
    }
}