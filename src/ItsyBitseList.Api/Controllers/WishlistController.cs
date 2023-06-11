using ItsyBitseList.Api.Models;
using ItsyBitseList.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItsyBitseList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WishlistController : ControllerBase
    {
        public WishlistController(IWishlistRepository wishlistCollectionRepository)
        {
            _wishlistCollectionRepository = wishlistCollectionRepository;
        }

        private IWishlistRepository _wishlistCollectionRepository { get; }

        [HttpGet("/wishlist", Name = "GetWishlists")]
        [ProducesResponseType(typeof(IEnumerable<WishlistOverview>), 200)]
        public IActionResult Get([FromHeader] string? owner)
        {
            try
            {

                var collection = _wishlistCollectionRepository.GetWishlistCollectionByOwner(owner);
                var result = collection.Select(wishlist => new WishlistOverview(wishlist.Id, wishlist.Name, wishlist.Items.Count));
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
        public IActionResult CreateWishlist([FromHeader] string? owner, [FromBody] WishlistCreationRequest request)
        {
            var id = Guid.NewGuid();
       
            _wishlistCollectionRepository.CreateWishlist(owner, id, request.Name);
        

            return CreatedAtRoute("GetWishlist", new { id }, null);
        }

        /// <summary>
        /// Retrieves a specific wishlist owned by the user
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/wishlist/{id}", Name = "GetWishlist")]
        [ProducesResponseType(typeof(WishlistDetails), 200)]
        public IActionResult GetWishlist([FromHeader] string? owner, [FromRoute] Guid id)
        {
            try
            {
                var result = _wishlistCollectionRepository.GetWishlist(id);
              
                return Ok(new WishlistDetails(result.Name, result.Items.Select(item => new Item(item.Id, item.State, item.Description)).ToList()));
            }
            catch (InvalidOperationException)
            {
                return NotFound($"Wishlist with id {id} no longer exists");
            }
        }

        [HttpPost("/wishlist/{id}/item", Name = "AddItemToWishlist")]
        public IActionResult Post([FromHeader] string owner, [FromRoute] Guid id, [FromBody] ItemCretionRequest item)
        {
            var itemId = Guid.NewGuid();
            var wishlist = _wishlistCollectionRepository.GetWishlist(id);
            wishlist.AddItem(itemId, item.Details);

            return CreatedAtRoute("GetItemInWishlist", new { id, itemId }, null);
        }

        /// <summary>
        /// Gets a specific item in a wishlist
        /// </summary> 
        [HttpGet("/wishlist/{id}/item/{itemId}", Name = "GetItemInWishlist")]
        public IActionResult GetItemInWishlist([FromHeader] string owner, [FromRoute] Guid id, [FromRoute] Guid itemId)
        {
            var wishlist = _wishlistCollectionRepository.GetWishlist(id);
            var item = wishlist.Items.First(x => x.Id == itemId);
            return Ok(new Item(item.Id, item.State, item.Description));
        }

        [HttpPatch("/wishlist/{id}/item/{itemId}", Name = "PromiseItemInWishlist")]
        public void Patch([FromHeader] string owner, [FromRoute] Guid id, [FromRoute] Guid itemId)
        {
            var wishlist = _wishlistCollectionRepository.GetWishlist(id);
            var item = wishlist.Items.First(x => x.Id == itemId);
            item.Promised();
        }

        [HttpDelete("/wishlist/{id}/item/{itemId}", Name = "DeleteItemInWishlist")]
        public void Delete([FromHeader] string owner, [FromRoute] Guid id, [FromRoute] Guid itemId)
        {
            var wishlist = _wishlistCollectionRepository.GetWishlist(id);
            wishlist.Remove(itemId);
        }
    }
}
