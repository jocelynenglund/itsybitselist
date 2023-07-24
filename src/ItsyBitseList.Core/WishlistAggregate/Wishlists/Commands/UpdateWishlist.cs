using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;
using static ItsyBitseList.Core.WishlistCollectionAggregate.Wishlist;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands
{
    public class UpdateWishlist
    {
        public record UpdateWishlistCommand(Guid Id, WishlistSettings Settings) : IRequest<WishlistSettings>;

        public class UpdateWishlistHandler : IRequestHandler<UpdateWishlistCommand, WishlistSettings>
        {
            private readonly IAsyncRepository<Wishlist> _wishlistRepository;
            public UpdateWishlistHandler(IAsyncRepository<Wishlist> wishlistRepository)
            {
                _wishlistRepository = wishlistRepository;   
            }
            public async Task<WishlistSettings> Handle(UpdateWishlistCommand request, CancellationToken cancellationToken)
            {
                var wishlist = await _wishlistRepository.GetByIdAsync(request.Id);
                wishlist.UpdateSettings(request.Settings);
                await _wishlistRepository.UpdateAsync(wishlist);

                return wishlist.AsWishlistSettings();
            }
        }
    }
}
