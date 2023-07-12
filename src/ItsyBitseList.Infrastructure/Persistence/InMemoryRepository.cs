using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Infrastructure.Persistence
{
    public class InMemoryRepository : IWishlistRepository, IAsyncRepository<Wishlist>, IAsyncRepository<WishlistItem>
    {
        
        public static Guid FirstId = Guid.Parse("f07223ff-ec05-4a86-90c6-81944377e71e");
        public static Guid SecondId = Guid.Parse("e1f39e36-2e0b-47ec-86f6-7ed53f9ce4c9");
        public static Guid MovieCard = Guid.Parse("f0307763-0e69-4e47-987a-353c1245e15d");
        public static Guid BarbieDoll = Guid.Parse("ab00b86c-6534-4f9d-8268-866117333a37");

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
            CreateWishlist("me", FirstId, "Birthday Wishlist");
            CreateWishlist("me", SecondId, "Christmas Wishlist");
            items.Add(new WishlistItem(MovieCard, FirstId, "Bio kort"));
            items.Add(new WishlistItem(BarbieDoll, FirstId, "Barbie Doll"));
            items.Add(new WishlistItem(Guid.NewGuid(), SecondId, "Robux"));
            items.Add(new WishlistItem(Guid.NewGuid(), SecondId, "Christmas dress"));
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
