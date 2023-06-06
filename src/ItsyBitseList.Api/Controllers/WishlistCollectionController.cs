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
        [HttpGet("{owner}", Name = "GetWishlistCollection")]
        [CustomExceptionFilter]
        public ActionResult<IEnumerable<WishlistListViewModel>> Get([FromRoute] string owner)
        {
            try
            {

                var result = _wishlistCollectionRepository.GetWishlistCollectionByOwner(owner);
                return Ok(result.Wishlists.Select(wishlist => new WishlistListViewModel(wishlist.Id, wishlist.Name, wishlist.Items.Count)));
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
        [HttpPost(Name = "CreateWishlistCollection")]
        public void Post([FromBody] WishlistCollectionCreationRequest request)
        {
            _wishlistCollectionRepository.CreateWishlistCollection(request.Owner);
        }

        /// <summary>
        /// Creates a new Wishlist owned by the user
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="wishlistName"></param>
        [HttpPost("{owner}/wishlist", Name = "CreateWishlist")]
        public void CreateWishlist([FromRoute] string owner, [FromBody] WishlistCreationRequest request)
        {
            var wishlistCollection = _wishlistCollectionRepository.GetWishlistCollectionByOwner(owner);
            wishlistCollection.CreateNewWishlist(request.Name);
        }

        [HttpGet("{owner}/wishlist/{id}", Name = "GetWishlist")]
        public ActionResult<WishlistDetailsViewModel> GetWishlist([FromRoute] string owner, [FromRoute] Guid id)
        {
            try
            {
                var wishlistCollection = _wishlistCollectionRepository.GetWishlistCollectionByOwner(owner);
                var result = wishlistCollection.Wishlists.First(x => x.Id == id);
                return Ok(new WishlistDetailsViewModel(result.Name, result.Items.Select(item => new Item(item)).ToList()));
            }
            catch (InvalidOperationException)
            {
                return NotFound($"Wishlist with id {id} no longer exists");
            }
        }
    }
}
