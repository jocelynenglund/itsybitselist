using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.CreateWishlist
{
    public record CreateWishlistCommand(
            string Owner,
            string WishlistName
        ) : IRequest<Guid>;

    public class CreateWishlistHandler : IRequestHandler<CreateWishlistCommand, Guid>
    {
        private readonly IAsyncRepository<Wishlist> _wishlistRepository;

        public CreateWishlistHandler(IAsyncRepository<Wishlist> wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task<Guid> Handle(CreateWishlistCommand request, CancellationToken cancellationToken)
        {
            var wishlist = Wishlist.CreateWith(request);
            wishlist = await _wishlistRepository.AddAsync(wishlist);
            return wishlist.Id;
        }
    }
}
