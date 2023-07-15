using ItsyBIT.Utilities;
using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries
{
    public class GetWishlist
    {
        public record Item(Guid Id, State State, string Description, Uri? Link);
        public record WishListDetails(string Name, IEnumerable<Item> Items, string PublicId);
        public record GetWishlistQuery(string Id) : IRequest<WishListDetails>;
        public class GetWishlistHandler : IRequestHandler<GetWishlistQuery, WishListDetails>
        {
            private readonly IAsyncRepository<Wishlist> _wishlistRepository;
            private EncodedIdentifierGenerator _generator { get; }  

            public GetWishlistHandler(IAsyncRepository<Wishlist> wishlistRepository, EncodedIdentifierGenerator generator)
            {
                _wishlistRepository = wishlistRepository;
                this._generator = generator;
            }
            public async Task<WishListDetails> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
            {
                var id =  Guid.TryParse(request.Id, out var parsedId) ? parsedId : _generator.Create(request.Id).Guid;
                var publicId = _generator.Create(id).EncodedShortKey;
                return (await _wishlistRepository.GetByIdAsync(id)).AsWishlistDetails(publicId);
            }
        }
    }
}
