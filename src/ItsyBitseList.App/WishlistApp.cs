using ItsyBitseList.Core.Interfaces.App;
using MediatR;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.AddItemToWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.CreateWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.DeleteItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.DeleteWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.PromiseItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.RevertPromiseItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist.GetItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;

namespace ItsyBitseList.App
{
    public class WishlistApp : IWishlistApp
    {
        private readonly IMediator _mediator;
        public WishlistApp(IMediator mediator)
        {
            _mediator = mediator;
        }


        public async Task<Response<Guid>> CreateWishlist(WishlistCreationRequest request)
        {
            var id = await _mediator.Send(new CreateWishlistCommand(request.Owner, request.Name));
            return new Response<Guid>(id);
        }

        public async Task<Response<Unit>> DeleteWishlist(Guid id, string? owner = null)
        {
            try
            {
                await _mediator.Send(new DeleteWishlistCommand(owner, id));

                return new Response<Unit>(Unit.Value);
            }
            catch (InvalidOperationException)
            {
                return new NotFoundErrorResponse<Unit>();
            }
            catch (Exception)
            {
                return new UnexpectedErrorResponse<Unit>();
            }
        }

        public async Task<Response<WishListDetails>> GetWishlist(string id)
        {
            try
            {
                var result = await _mediator.Send(new GetWishlistQuery(id));

                return new Response<WishListDetails>(result);
            }
            catch (InvalidOperationException)
            {
                return new NotFoundErrorResponse<WishListDetails>();
            }
            catch (Exception)
            {
                return new UnexpectedErrorResponse<WishListDetails>();
            }
        }

        public async Task<Response<Guid>> AddItemToWishlist(Guid id, ItemCreationRequest request)
        {

            try
            {
                var result = (await _mediator.Send(new AddItemToWishlistCommand(id, request.Details, request.Link)));

                return new Response<Guid>(result);
            }
            catch (Exception)
            {
                return new UnexpectedErrorResponse<Guid>();
            }

        }

        public async Task<Response<ItemDetails>> GetItemInWishlist(string wishlistId, Guid itemId)
        {
            try
            {
                var item = await _mediator.Send(new GetItemInWishlistQuery(wishlistId, itemId));

                return new Response<ItemDetails>(item);
            }
            catch (InvalidOperationException)
            {
                return new NotFoundErrorResponse<ItemDetails>();
            }
            catch (Exception)
            {
                return new UnexpectedErrorResponse<ItemDetails>();
            }
        }

        public async Task<Response<Unit>> DeleteWishlistItem(Guid wishlistId, Guid itemId)
        {
            try
            {
                await _mediator.Send(new DeleteItemInWishlistCommand(wishlistId, itemId));
                return new Response<Unit>(Unit.Value);
            }
            catch (InvalidOperationException)
            {
                return new NotFoundErrorResponse<Unit>();
            }
            catch (Exception)
            {
                return new UnexpectedErrorResponse<Unit>();
            }
        }

        public async Task<Response<Guid>> PromiseItem(string wishlistId, Guid itemId)
        {
            try
            {
                var promised = await _mediator.Send(new PromiseItemInWishlistCommand(wishlistId, itemId));
                return new Response<Guid>(promised);
            }
            catch (InvalidOperationException)
            {
                return new NotFoundErrorResponse<Guid>();
            }
            catch (Exception)
            {
                return new UnexpectedErrorResponse<Guid>();
            }
        }

        public async Task<Response<Guid>> RevertPromise(string wishlistId, Guid itemId, Guid promiseKey)
        {
            try
            {
                await _mediator.Send(new RevertPromiseItemInWishlistCommand(wishlistId, itemId, promiseKey));
                return new Response<Guid>(promiseKey);
            }
            catch (InvalidOperationException)
            {
                return new NotFoundErrorResponse<Guid>();
            }
            catch (Exception)
            {
                return new UnexpectedErrorResponse<Guid>();
            }
        }
    }
}
