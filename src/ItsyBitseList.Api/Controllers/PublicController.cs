using ItsyBitseList.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.PromiseItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.RevertPromiseItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist.GetItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;

namespace ItsyBitseList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicController: ControllerBase
    {
        private readonly IMediator _mediator;
        public PublicController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a specific wishlist owned by the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/public/{id}", Name = "GetPublicWishlist")]
        [ProducesResponseType(typeof(WishListDetails), 200)]
        public async Task<IActionResult> GetWishlist( [FromRoute] string id)
        {
            try
            { 
                var result = await _mediator.Send(new GetWishlistQuery(id));

                return Ok(result);
            }
            catch (InvalidOperationException)
            {
                return NotFound($"Wishlist with id {id} no longer exists");
            }
        }
        [HttpPatch("/public/{id}/item/{itemId}", Name = "PromiseItemInWishlist")]
        public async Task<IActionResult> Patch([FromRoute] string id, [FromRoute] Guid itemId, [FromBody] PatchRequest request)
        {
            if (request.State == "Promised")
            {
                var promised = await _mediator.Send(new PromiseItemInWishlistCommand(id, itemId));
                return Ok(promised);
            }
            if (request.PromiseKey.HasValue)
            {
                await _mediator.Send(new RevertPromiseItemInWishlistCommand(id, itemId, request.PromiseKey.Value));
                return Ok(request.PromiseKey.Value);
            }

            return NoContent();
        }


        /// <summary>
        /// Gets a specific item in a wishlist
        /// </summary> 
        [HttpGet("/public/{id}/item/{itemId}", Name = "GetItemInPublicWishlist")]
        [ProducesResponseType(typeof(ItemDetails), 200)]
        public async Task<IActionResult> GetItemInWishlist([FromHeader] string? owner, [FromRoute] string id, [FromRoute] Guid itemId)
        {
            try
            {
                var item = await _mediator.Send(new GetItemInWishlistQuery(id, itemId));

                return Ok(item);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }

        }
    }
}
