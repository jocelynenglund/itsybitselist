using ItsyBitseList.Api.Filters;
using ItsyBitseList.Api.Models;
using ItsyBitseList.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItsyBitseList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WishlistCollectionController : ControllerBase
    {
        public WishlistCollectionController(IWishlistCollectionRepository wishlistCollectionRepository)
        {
            _wishlistCollectionRepository = wishlistCollectionRepository;
        }

        private IWishlistCollectionRepository _wishlistCollectionRepository { get; }


        /// <summary>
        /// Gets a list of wishlists owned by the user
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomExceptionFilter]
        [ProducesResponseType(typeof(IEnumerable<WishlistOverview>), 200)]
        public IActionResult Get([FromHeader] string owner)
        {
            try
            {

                var collection = _wishlistCollectionRepository.GetWishlistCollectionByOwner(owner);
                var result = collection.Wishlists.Select(wishlist => new WishlistOverview(wishlist.Id, wishlist.Name, wishlist.Items.Count));
                return Ok(result);
            }
            catch (InvalidOperationException)
            {

                return NotFound($"Unable to find wishlist for {owner}");
            }
        }

        /// <summary>
        /// Creates a new WishlistCollection owned by the user
        /// </summary>
        /// <param name="owner"></param>
        [HttpPost]
        public IActionResult Post([FromHeader] string owner, [FromBody] WishlistCollectionCreationRequest request)
        {
            var id = Guid.NewGuid();
            _wishlistCollectionRepository.CreateWishlistCollection(owner, id, request.WishlistName);
            return CreatedAtRoute("GetWishlist", new { id }, null);
        }

     
    }
}
