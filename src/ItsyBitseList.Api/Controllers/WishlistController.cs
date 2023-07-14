using ItsyBitseList.Api.Models;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.AddItemToWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.CreateWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.DeleteItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist.GetItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlists;

namespace ItsyBitseList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WishlistController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("/wishlist", Name = "GetWishlists")]
        [ProducesResponseType(typeof(IEnumerable<WishlistOverview>), 200)]
        public async Task<IActionResult> Get([FromHeader] string? owner)
        {
            try
            {

                var result = await _mediator.Send(new GetWishlistsQuery(owner));

                return Ok(result);
            }
            catch (InvalidOperationException)
            {

                return NotFound($"Unable to find wishlist for {owner}");
            }
        }
        /// <summary>
        /// Creates a new Wishlist owned by the user
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="wishlistName"></param>
        [HttpPost("/wishlist", Name = "CreateWishlist")]
        public async Task<IActionResult> CreateWishlist([FromHeader] string? owner, [FromBody] WishlistCreationRequest request)
        {
            var id = await _mediator.Send(new CreateWishlistCommand(owner, request.Name));

            return CreatedAtRoute("GetWishlist", new { id }, null);
        }

        /// <summary>
        /// Retrieves a specific wishlist owned by the user
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/wishlist/{id}", Name = "GetWishlist")]
        [ProducesResponseType(typeof(WishListDetails), 200)]
        public async Task<IActionResult> GetWishlist([FromHeader] string? owner, [FromRoute] Guid id)
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

        /// <summary>
        /// Deletes a specific wishlist owned by the user
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/wishlist/{id}", Name = "DeleteWishlist")]
        public async Task<IActionResult> DeleteWishlist([FromHeader] string? owner, [FromRoute] Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteWishlistCommand(owner, id));

                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound($"Wishlist with id {id} no longer exists");
            }
        }

        [HttpPost("/wishlist/{id}/item", Name = "AddItemToWishlist")]
        public async Task<IActionResult> Post([FromHeader] string? owner, [FromRoute] Guid id, [FromBody] ItemCreationRequest item)
        {
            var itemId = (await _mediator.Send(new AddItemToWishlistCommand(id, item.Details, item.Link)));

            return CreatedAtRoute("GetItemInWishlist", new { id, itemId }, null);
        }

        /// <summary>
        /// Gets a specific item in a wishlist
        /// </summary> 
        [HttpGet("/wishlist/{id}/item/{itemId}", Name = "GetItemInWishlist")]
        [ProducesResponseType(typeof(ItemDetails), 200)]
        public async Task<IActionResult> GetItemInWishlist([FromHeader] string? owner, [FromRoute] Guid id, [FromRoute] Guid itemId)
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


        [HttpDelete("/wishlist/{id}/item/{itemId}", Name = "DeleteItemInWishlist")]
        public async Task<IActionResult> Delete([FromHeader] string? owner, [FromRoute] Guid id, [FromRoute] Guid itemId)
        {
            await _mediator.Send(new DeleteItemInWishlistCommand(id, itemId));

            return NoContent();
        }
    }
}
