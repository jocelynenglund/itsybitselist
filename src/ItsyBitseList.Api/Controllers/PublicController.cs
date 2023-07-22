using ItsyBitseList.App;
using ItsyBitseList.Core.Interfaces.App;
using Microsoft.AspNetCore.Mvc;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist.GetItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;

namespace ItsyBitseList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicController: ControllerBase
    {
        private readonly IWishlistApp _application;
        public PublicController(IWishlistApp application)
        {
            _application = application;
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
            var response = await _application.GetWishlist(id);
            return response.AsActionResult();
        }

        [HttpPatch("/public/{id}/item/{itemId}", Name = "PromiseItemInWishlist")]
        public async Task<IActionResult> Patch([FromRoute] string id, [FromRoute] Guid itemId, [FromBody] PatchRequest request)
        {
            if (request.State == "Promised")
            {
                var response = await _application.PromiseItem(id, itemId);
                return response.AsActionResult();
            }
            if (request.PromiseKey.HasValue)
            {
                var response = await _application.RevertPromise(id, itemId, request.PromiseKey.Value);
                return response.AsActionResult();
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
            var response = await _application.GetItemInWishlist(id, itemId);

            return response.AsActionResult();
        }
    }
}
