using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItsyBitseList.IntegrationTests
{
    public class MockRepo : IWishlistRepository, IAsyncRepository<Wishlist>, IAsyncRepository<WishlistItem>
    {
        public Task<Wishlist> AddAsync(Wishlist entity)
        {
            throw new NotImplementedException();
        }

        public Task<WishlistItem> AddAsync(WishlistItem entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Wishlist entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(WishlistItem entity)
        {
            throw new NotImplementedException();
        }

        public Task<Wishlist> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Wishlist>> GetWishlistByOwnerAsync(string owner)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Wishlist>> ListAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Wishlist entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(WishlistItem entity)
        {
            throw new NotImplementedException();
        }

        Task<WishlistItem> IAsyncRepository<WishlistItem>.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<IReadOnlyList<WishlistItem>> IAsyncRepository<WishlistItem>.ListAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
