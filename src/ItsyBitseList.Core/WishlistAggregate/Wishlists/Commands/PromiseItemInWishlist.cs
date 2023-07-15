using ItsyBIT.Utilities;
using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands
{
    public class PromiseItemInWishlist
    {
        public record class PromiseItemInWishlistCommand(string WishlistId, Guid ItemId) : IRequest<Guid>;
        public class PromiseItemInWishlistHandler : IRequestHandler<PromiseItemInWishlistCommand, Guid>
        {
            private readonly IAsyncRepository<Wishlist> _repository;
            private EncodedIdentifierGenerator _generator;
            public PromiseItemInWishlistHandler(IAsyncRepository<Wishlist> repository, EncodedIdentifierGenerator generator)
            {
                _generator = generator;
                _repository = repository;

            }
            public async Task<Guid> Handle(PromiseItemInWishlistCommand request, CancellationToken cancellationToken)
            {
                var wishlistId = Guid.TryParse(request.WishlistId, out var parsedId)
                ? parsedId
                : _generator.Create(request.WishlistId).Guid;
                var wishlist = await _repository.GetByIdAsync(wishlistId);
                var toPromise = wishlist.Items.First(i => i.Id == request.ItemId);
                if (toPromise.WishlistId != wishlistId)
                {
                    throw new UnauthorizedAccessException("Item not found in wishlist");
                }
                else
                {
                    var result = toPromise.Promised();
                    await _repository.UpdateAsync(wishlist);

                    return result;
                }
            }
        }
    }

}
