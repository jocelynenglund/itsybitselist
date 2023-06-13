using ItsyBitseList.Api.Models;
using ItsyBitseList.Core.Interfaces;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.AddItemToWishlist;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.CreateWishlist;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.DeleteItemInWishlist;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.PromiseItemInWishlist;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlists;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItsyBitseList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WishlistController : ControllerBase
    {
        private IWishlistRepositoryLegacy _wishlistCollectionRepository { get; }
        private readonly IMediator _mediator;

        public WishlistController(IWishlistRepositoryLegacy wishlistCollectionRepository, IMediator mediator)
        {
            _mediator = mediator;
            _wishlistCollectionRepository = wishlistCollectionRepository;
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

        [HttpPost("/wishlist/{id}/item", Name = "AddItemToWishlist")]
        public async Task<IActionResult> Post([FromHeader] string? owner, [FromRoute] Guid id, [FromBody] ItemCretionRequest item)
        {
            var itemId = (await _mediator.Send(new AddItemToWishlistCommand(id, item.Details)));

            return CreatedAtRoute("GetItemInWishlist", new { id, itemId }, null);
        }

        /// <summary>
        /// Gets a specific item in a wishlist
        /// </summary> 
        [HttpGet("/wishlist/{id}/item/{itemId}", Name = "GetItemInWishlist")]
        public async Task<IActionResult> GetItemInWishlist([FromHeader] string? owner, [FromRoute] Guid id, [FromRoute] Guid itemId)
        {
            var item = await _mediator.Send(new GetItemInWishlistQuery(id, itemId));
            
            return Ok(item);
        }

        [HttpPatch("/wishlist/{id}/item/{itemId}", Name = "PromiseItemInWishlist")]
        public async Task<IActionResult> Patch([FromRoute] Guid id, [FromRoute] Guid itemId)
        {
            await _mediator.Send(new PromiseItemInWishlistCommand(id, itemId));

            return NoContent();
        }

        [HttpDelete("/wishlist/{id}/item/{itemId}", Name = "DeleteItemInWishlist")]
        public async Task<IActionResult> Delete([FromHeader] string owner, [FromRoute] Guid id, [FromRoute] Guid itemId)
        {
            await _mediator.Send(new DeleteItemInWishlistCommand(id, itemId));

            return NoContent();
        }
    }
}
