using ItsyBitseList.Core.Constants;
using ItsyBitseList.Core.Interfaces.Persistence;
using MediatR;

namespace ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands
{
    public record DeleteWishlistCommand(string Owner, Guid WishlistId) : IRequest;
    public class DeleteWishlistCommandHandler : IRequestHandler<DeleteWishlistCommand>
    {
        private readonly IWishlistRepository _repository;
        public DeleteWishlistCommandHandler(IWishlistRepository repository)
        {
            _repository = repository;
        }
        public async Task Handle(DeleteWishlistCommand request, CancellationToken cancellationToken)
        {
            var wishlist = await _repository.GetByIdAsync(request.WishlistId) ?? throw new InvalidOperationException(ErrorMessages.WishlistNotFound);

            await _repository.DeleteAsync(wishlist);
        }
    }
}
