﻿using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.IntegrationTests.TestObjects
{
    public class InMemoryRepository : IWishlistRepository, IAsyncRepository<Wishlist>
    {

        public static Guid FirstId = Guid.Parse("9dc2205a-2ff0-4ae4-a48f-5557be2485bf");
        public static Guid SecondId = Guid.Parse("e1f39e36-2e0b-47ec-86f6-7ed53f9ce4c9");
        public static Guid MovieCard = Guid.Parse("f0307763-0e69-4e47-987a-353c1245e15d");
        public static Guid BarbieDoll = Guid.Parse("ab00b86c-6534-4f9d-8268-866117333a37");

        List<Wishlist> wishlists;
        public IEnumerable<Wishlist> Wishhlists => wishlists;
        public InMemoryRepository(bool seeded = true)
        {
            wishlists = new List<Wishlist>();
            if (seeded) SeedData();
        }

        private void SeedData()
        {
            CreateWishlist("me", FirstId, "Birthday Wishlist");
            CreateWishlist("me", SecondId, "Christmas Wishlist");
            wishlists.First(w => w.Id == FirstId).SetItems(new List<WishlistItem>()
            {
                new WishlistItem(MovieCard, FirstId, "Bio kort"),
                new WishlistItem(BarbieDoll, FirstId, "Barbie Doll")
            });
            wishlists.First(w => w.Id == SecondId).SetItems(new List<WishlistItem>()
            {
                new WishlistItem(Guid.NewGuid(), SecondId, "Robux"),
                new WishlistItem(Guid.NewGuid(), SecondId, "Christmas dress")
            });
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
            var wishlist = wishlists.First(item => item.Id == id);
            return Task.FromResult(wishlist);
        }

        public Task<IReadOnlyList<Wishlist>> ListAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Wishlist>>(wishlists);
        }

        public Task<Wishlist> AddAsync(Wishlist entity)
        {
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
    }
}
