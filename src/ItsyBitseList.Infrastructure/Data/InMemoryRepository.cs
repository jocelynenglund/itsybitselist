using ItsyBitseList.Core.Interfaces;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using System.ComponentModel;

namespace ItsyBitseList.Infrastructure.Data
{
    public class InMemoryRepository : IWishlistRepository
    {
        List<Wishlist> wishlists;
        public IEnumerable<Wishlist> Wishhlists => wishlists;
        public InMemoryRepository(bool seeded = true)
        {
            wishlists = new List<Wishlist>();
            if (seeded) SeedData();
        }

        private void SeedData()
        {
            var one = Guid.NewGuid();
            var two = Guid.NewGuid();
            CreateWishlist("me", one, "Birthday Wishlist");
            CreateWishlist("me", two, "Christmas Wishlist");
            wishlists.First(x=>x.Id==one).AddItem(Guid.NewGuid(), "Bio kort");
            wishlists.First(x => x.Id == one).AddItem(Guid.NewGuid(), "Barbie Doll");
            wishlists.First(x => x.Id == two).AddItem(Guid.NewGuid(), "Robux");
            wishlists.First(x => x.Id == two).AddItem(Guid.NewGuid(), "Christmas dress");
        }

      
        public void CreateWishlist(string owner, Guid id, string wishlistName)
        {
            wishlists.Add(Wishlist.CreateWith(id, wishlistName, owner));
        }

        IEnumerable<Wishlist> IWishlistRepository.GetWishlistCollectionByOwner(string? owner)
        {
            return wishlists.Where(item => owner!=null? item.Owner == owner: true).ToList();
        }

        public Wishlist GetWishlist(Guid id)
        {
            return wishlists.First(item => item.Id == id);
        }   
    }
}
