using ItsyBIT.Utilities;
using ItsyBitseList.Core.Constants;
using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;
using System.Reflection.Emit;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands
{
    public class RevertPromiseItemInWishlist
    {
        public record class RevertPromiseItemInWishlistCommand(string WishlistId, Guid ItemId, Guid PromisedBy): IRequest;

        public class RevertPromiseItemInWishlistHandler : IRequestHandler<RevertPromiseItemInWishlistCommand>
        {
            private readonly IAsyncRepository<Wishlist> _repository;
            private readonly EncodedIdentifierGenerator _generator;
            public RevertPromiseItemInWishlistHandler(IAsyncRepository<Wishlist> repository, EncodedIdentifierGenerator generator)
            {
                _repository = repository;
                _generator = generator;
            }
            public async Task Handle(RevertPromiseItemInWishlistCommand request, CancellationToken cancellationToken)
            {
                var wishlistId = Guid.TryParse(request.WishlistId, out var parsedId)
                ? parsedId
                : _generator.Create(request.WishlistId).Guid;
                var wishlist = await _repository.GetByIdAsync(wishlistId);

                var toRevert = wishlist.Items.First(i => i.Id == request.ItemId);
                if (toRevert.WishlistId != wishlistId)
                {
                    throw new InvalidOperationException(ErrorMessages.ItemNotFound);
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
