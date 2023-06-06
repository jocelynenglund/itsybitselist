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
        public void Post([FromHeader] string owner, [FromBody] WishlistCollectionCreationRequest request)
        {
            _wishlistCollectionRepository.CreateWishlistCollection(owner, request.WishlistName);
        }

        /// <summary>
        /// Creates a new Wishlist owned by the user
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="wishlistName"></param>
        [HttpPost("/wishlist", Name = "CreateWishlist")]
        public void CreateWishlist([FromHeader] string owner, [FromBody] WishlistCreationRequest request)
        {
            var wishlistCollection = _wishlistCollectionRepository.GetWishlistCollectionByOwner(owner);
            wishlistCollection.CreateNewWishlist(request.Name);
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
                return Ok(new WishlistDetails(result.Name, result.Items.Select(item => new Item(item.Id,item.Description)).ToList()));
            }
            catch (InvalidOperationException)
            {
                return NotFound($"Wishlist with id {id} no longer exists");
            }
        }
    }
}
