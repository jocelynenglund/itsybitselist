﻿using ItsyBitseList.Core.WishlistCollectionAggregate;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist.GetItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;
using static ItsyBitseList.Core.WishlistCollectionAggregate.Wishlist;

namespace ItsyBitseList.Core
{
    public static class MappingExtensions
    {
        public static WishListDetails AsWishlistDetails(this Wishlist wishlist, string publicId)
        {
            return new WishListDetails(
                wishlist.Name, 
                wishlist.Items.Select(x => new Item(x.Id, x.State, x.Description, x.Link)),
                publicId,
                wishlist.Description
                );
        }
        public static WishlistSettings AsWishlistSettings(this Wishlist wishlist)
        {
            return new WishlistSettings(
                wishlist.Name,
                wishlist.Description
                );
        }

        public static ItemDetails AsItemDetails(this WishlistItem item)
        {
            return new ItemDetails(item.Id, item.State, item.Description);
        }
    }
}
