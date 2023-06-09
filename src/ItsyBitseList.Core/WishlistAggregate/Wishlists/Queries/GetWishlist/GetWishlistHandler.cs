﻿using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist
{
    public record Item(Guid Id, State State, string Description, Uri? Link);
    public record WishListDetails(string Name, IEnumerable<Item> Items);
    public record GetWishlistQuery(Guid Id): IRequest<WishListDetails>;
    public class GetWishlistHandler : IRequestHandler<GetWishlistQuery, WishListDetails>
    {
        private readonly IAsyncRepository<Wishlist> _wishlistRepository;

        public GetWishlistHandler(IAsyncRepository<Wishlist> wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }
        public async Task<WishListDetails> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
        {
            return (await _wishlistRepository.GetByIdAsync(request.Id)).AsWishlistDetails();
        }
    }
}
