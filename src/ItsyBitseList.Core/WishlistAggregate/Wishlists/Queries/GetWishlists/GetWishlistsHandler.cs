using ItsyBitseList.Core.Interfaces.Persistence;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlists
{
    /// <summary>
    /// Summary of a wishlist collection
    /// </summary>
    /// <param name="Id">Guid to uniquely identify wishlist collection</param>
    /// <param name="Title">Title</param>
    /// <param name="NumberOfItems">Number of items in the wishlist</param>
    public record WishlistOverview(
        Guid Id,
        string Title,
        int ItemCount
    );

    public record GetWishlistsQuery(
        string Owner
    ) : IRequest<IEnumerable<WishlistOverview>>;

    public class GetWishlistsHandler : IRequestHandler<GetWishlistsQuery, IEnumerable<WishlistOverview>>
    {
        private readonly IWishlistRepository _wishlistRepository;
        public GetWishlistsHandler(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task<IEnumerable<WishlistOverview>> Handle(GetWishlistsQuery request, CancellationToken cancellationToken)
        {
            var wishLists = (await _wishlistRepository.GetWishlistByOwnerAsync(request.Owner));
            return wishLists.Select(x => new WishlistOverview(x.Id, x.Name, x.Items.Count));
        }
    }
}
