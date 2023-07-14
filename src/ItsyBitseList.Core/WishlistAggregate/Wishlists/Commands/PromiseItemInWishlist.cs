using ItsyBIT.Utilities;
using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands
{
    public class PromiseItemInWishlist
    {
        public record class PromiseItemInWishlistCommand(Guid WishlistId, Guid ItemId) : IRequest<Guid>
        {
            public PromiseItemInWishlistCommand(string publicId, Guid itemId) :
                this(
                    Guid.TryParse(publicId, out var parsedId)
                        ? parsedId
                        : new EncodedIdentifier(publicId).Guid,
                    itemId)
            { }
        };
        public class PromiseItemInWishlistHandler : IRequestHandler<PromiseItemInWishlistCommand, Guid>
        {
            private readonly IAsyncRepository<Wishlist> _repository;
            public PromiseItemInWishlistHandler(IAsyncRepository<Wishlist> repository)
            {

                _repository = repository;

            }
            public async Task<Guid> Handle(PromiseItemInWishlistCommand request, CancellationToken cancellationToken)
            {
                var wishlist = await _repository.GetByIdAsync(request.WishlistId);
                var toPromise = wishlist.Items.First(i => i.Id == request.ItemId);
                if (toPromise.WishlistId != request.WishlistId)
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
