namespace ItsyBitseList.Core.WishlistCollectionAggregate
{
    public class WishlistCollection
    {
        public List<Wishlist> Wishlists { get; set; }
        public string Owner { get; set; }

        public WishlistCollection(string owner)
        {
            Wishlists = new List<Wishlist>();
            Owner = owner;
        }

        public void CreateNewWishlist(Guid id, string Title)
        {
            Wishlists.Add(Wishlist.CreateWith(id, Title));
        }

    }
}
