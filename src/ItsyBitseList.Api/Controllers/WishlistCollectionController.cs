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

        [HttpGet("{owner}", Name = "GetWishlistCollection")]
    
        public IEnumerable<WishlistListViewModel> Get([FromRoute] string owner)
        {
            var result = _wishlistCollectionRepository.GetWishlistCollectionByOwner(owner);
            return result.Wishlists.Select(wishlist => new WishlistListViewModel(wishlist.Name, wishlist.Items.Count));
        }

        [HttpPost(Name ="CreateWishlistCollection")]
        public void Post([FromBody] string owner)
        {
            _wishlistCollectionRepository.CreateWishlistCollection(owner);
        }
    }
}
