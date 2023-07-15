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
        public record GetItemInWishlistQuery(string WishlistId, Guid ItemId) : IRequest<ItemDetails>;

        public class GetItemInWishlistHandler : IRequestHandler<GetItemInWishlistQuery, ItemDetails>
        {
            private readonly IAsyncRepository<Wishlist> _repository;
            EncodedIdentifierGenerator _generator;
            public GetItemInWishlistHandler(IAsyncRepository<Wishlist> repository, EncodedIdentifierGenerator generator)
            {
                _repository = repository;
                _generator = generator;
            }
            public async Task<ItemDetails> Handle(GetItemInWishlistQuery request, CancellationToken cancellationToken)
            {
                var wishlistId = Guid.TryParse(request.WishlistId, out var parsedId)
                        ? parsedId
                        : _generator.Create(request.WishlistId).Guid;
                var result = (await _repository.GetByIdAsync(wishlistId)).Items.FirstOrDefault(i => i.Id == request.ItemId);

                if (result == null || result.WishlistId != wishlistId)
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