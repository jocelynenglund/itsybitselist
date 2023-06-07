using ItsyBitseList.Api.Models;
using ItsyBitseList.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItsyBitseList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WishlistController : ControllerBase
    {
        public WishlistController(IWishlistCollectionRepository wishlistCollectionRepository)
        {
            _wishlistCollectionRepository = wishlistCollectionRepository;
        }

        private IWishlistCollectionRepository _wishlistCollectionRepository { get; }

        /// <summary>
        /// Creates a new Wishlist owned by the user
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="wishlistName"></param>
        [HttpPost("/wishlist", Name = "CreateWishlist")]
        public IActionResult CreateWishlist([FromHeader] string owner, [FromBody] WishlistCreationRequest request)
        {
            var id = Guid.NewGuid();
            var wishlistCollection = _wishlistCollectionRepository.GetWishlistCollectionByOwner(owner);
            wishlistCollection.CreateNewWishlist(id, request.Name);
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
        public IActionResult GetWishlist([FromHeader] string owner, [FromRoute] Guid id)
        {
            try
            {
                var wishlistCollection = _wishlistCollectionRepository.GetWishlistCollectionByOwner(owner);
                var result = wishlistCollection.Wishlists.First(x => x.Id == id);
                return Ok(new WishlistDetails(result.Name, result.Items.Select(item => new Item(item.Id, item.Description)).ToList()));
            }
            catch (InvalidOperationException)
            {
                return NotFound($"Wishlist with id {id} no longer exists");
            }
        }

        [HttpPost("/wishlist/{id}/item", Name = "AddItemToWishlist")]
        public IActionResult Post([FromHeader] string owner, [FromRoute] Guid id, [FromBody] ItemCretionRequest item )
        {
            var itemId = Guid.NewGuid();
            var wishlistCollection = _wishlistCollectionRepository.GetWishlistCollectionByOwner(owner);
            var wishlist = wishlistCollection.Wishlists.First(x => x.Id == id);
            wishlist.AddItem(itemId, item.Details);

            return CreatedAtRoute("GetItemInWishlist", new { id, itemId }, null);
        }

        /// <summary>
        /// Gets a specific item in a wishlist
        /// </summary> 
        [HttpGet("/wishlist/{id}/item/{itemId}", Name = "GetItemInWishlist")]
        public IActionResult GetItemInWishlist([FromHeader] string owner, [FromRoute] Guid id, [FromRoute] Guid itemId)
        {
            var wishlistCollection = _wishlistCollectionRepository.GetWishlistCollectionByOwner(owner);
            var wishlist = wishlistCollection.Wishlists.First(x => x.Id == id);
            var item = wishlist.Items.First(x => x.Id == itemId);
            return Ok(new Item(item.Id, item.Description));
        }
    }
}
