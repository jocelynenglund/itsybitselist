using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Infrastructure.Persistence
{
    public class InMemoryRepository : IWishlistRepository, IAsyncRepository<Wishlist>, IAsyncRepository<WishlistItem>
    {
        List<Wishlist> wishlists;
        public IEnumerable<Wishlist> Wishhlists => wishlists;
        public List<WishlistItem> items = new List<WishlistItem>();
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
            items.Add(new WishlistItem(Guid.NewGuid(), one, "Bio kort"));
            items.Add(new WishlistItem(Guid.NewGuid(), one, "Barbie Doll"));
            items.Add(new WishlistItem(Guid.NewGuid(), two, "Robux"));
            items.Add(new WishlistItem(Guid.NewGuid(), two, "Christmas dress"));
        }


        public void CreateWishlist(string owner, Guid id, string wishlistName)
        {
            wishlists.Add(Wishlist.CreateWith(id, wishlistName, owner));
        }

        public Wishlist GetWishlist(Guid id)
        {
            return wishlists.First(item => item.Id == id);
        }

        public Task<IEnumerable<Wishlist>> GetWishlistByOwnerAsync(string owner)
        {
            return Task.FromResult(wishlists.Where(wishlists => wishlists.Owner == owner));
        }

        public Task<Wishlist> GetByIdAsync(Guid id)
        {
            var wishlistItems = items.Where(item => item.WishlistId == id);
            var wishlist = wishlists.First(item => item.Id == id);
            wishlist.SetItems(wishlistItems.ToList());
            return Task.FromResult(wishlist);
        }

        public Task<IReadOnlyList<Wishlist>> ListAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Wishlist>>(wishlists);
        }

        public Task<Wishlist> AddAsync(Wishlist entity)
        {
            entity.Id = Guid.NewGuid();
            wishlists.Add(entity);
            return Task.FromResult(entity);
        }

        public Task UpdateAsync(Wishlist entity)
        {
            wishlists.Remove(wishlists.First(item => item.Id == entity.Id));
            wishlists.Add(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Wishlist entity)
        {
            wishlists.Remove(wishlists.First(item => item.Id == entity.Id));
            return Task.CompletedTask;
        }

        Task<WishlistItem> IAsyncRepository<WishlistItem>.GetByIdAsync(Guid id)
        {
           return Task.FromResult(items.First(item => item.Id == id));
        }

        Task<IReadOnlyList<WishlistItem>> IAsyncRepository<WishlistItem>.ListAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WishlistItem> AddAsync(WishlistItem entity)
        {
            entity = entity with { Id = Guid.NewGuid() };
            items.Add(entity);
            return Task.FromResult(entity);
        }

        public Task UpdateAsync(WishlistItem entity)
        {
            items.Remove(items.First(item => item.Id == entity.Id));
            items.Add(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(WishlistItem entity)
        {
            items.Remove(items.First(items => items.Id == entity.Id));
            return Task.CompletedTask;
        }
    }
}
