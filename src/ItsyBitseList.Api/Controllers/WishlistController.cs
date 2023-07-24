using ItsyBitseList.App;
using ItsyBitseList.Core.Interfaces.App;
using Microsoft.AspNetCore.Mvc;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist.GetItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;

namespace ItsyBitseList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistApp _application;

        public WishlistController(IWishlistApp application)
        {
            _application = application;
        }

        /// <summary>
        /// Creates a new Wishlist owned by the user
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="wishlistName"></param>
        [HttpPost("/wishlist", Name = "CreateWishlist")]
        public async Task<IActionResult> CreateWishlist([FromHeader] string? owner, [FromBody] WishlistCreationRequest request)
        {
            var response = await _application.CreateWishlist(request);

            return CreatedAtRoute("GetWishlist", new { id = response.Result }, null);
        }
        /// <summary>
        /// Updates a new Wishlist owned by the user
        /// </summary>
        /// <param name="wishlistName"></param>
        [HttpPatch("/wishlist/{id}", Name = "UpdateWishlist")]
        public async Task<IActionResult> UpdateWishlist([FromRoute] Guid id, [FromBody] WishlistUpdateRequest request)
        {
            var response = await _application.UpdateWishlist(id, request);

            return response.AsActionResult();
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
            var response = await _application.GetWishlist(id.ToString());
            return response.AsActionResult();

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
            var response = await _application.DeleteWishlist(id, owner);

            return response.AsActionResult();
        }

        [HttpPost("/wishlist/{id}/item", Name = "AddItemToWishlist")]
        public async Task<IActionResult> Post([FromHeader] string? owner, [FromRoute] Guid id, [FromBody] ItemCreationRequest item)
        {
            var response = await _application.AddItemToWishlist(id, item);

            return CreatedAtRoute("GetItemInWishlist", new { id, itemId = response.Result }, null);
        }

        /// <summary>
        /// Gets a specific item in a wishlist
        /// </summary> 
        [HttpGet("/wishlist/{id}/item/{itemId}", Name = "GetItemInWishlist")]
        [ProducesResponseType(typeof(ItemDetails), 200)]
        public async Task<IActionResult> GetItemInWishlist([FromHeader] string? owner, [FromRoute] string id, [FromRoute] Guid itemId)
        {
            var response = await _application.GetItemInWishlist(id, itemId);

            return response.AsActionResult();
        }


        [HttpDelete("/wishlist/{id}/item/{itemId}", Name = "DeleteItemInWishlist")]
        public async Task<IActionResult> Delete([FromHeader] string? owner, [FromRoute] Guid id, [FromRoute] Guid itemId)
        {
            var response = await _application.DeleteWishlistItem(id, itemId);

            return response.AsActionResult();
        }
    }
}
