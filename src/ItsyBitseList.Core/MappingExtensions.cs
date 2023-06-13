using ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;
using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Core
{
    public static class MappingExtensions
    {
        public static WishListDetails AsWishlistDetails(this Wishlist wishlist)
        {
            return new WishListDetails(wishlist.Name, wishlist.Items.Select(x => new Item(x.Id, x.State, x.Description)));
        }

        public static ItemDetails AsItemDetails(this WishlistItem item)
        {
            return new ItemDetails(item.Id, item.State, item.Description);
        }
    }
}
