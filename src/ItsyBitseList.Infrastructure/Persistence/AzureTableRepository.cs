using Azure;
using Azure.Data.Tables;
using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace ItsyBitseList.Infrastructure.Persistence
{

    public class WishlistEntity : ITableEntity
    {
        public string PartitionKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string RowKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeOffset? Timestamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ETag ETag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
    public class AzureTableRepository : IWishlistRepository, IAsyncRepository<Wishlist>, IAsyncRepository<WishlistItem>
    {
        private TableClient _tableClient;
        private string _wishlistTableName;
        private string _itemTableName;
        private string _connectionString;

        public AzureTableRepository(IOptions<StorageSettings> options)
        {
            var settings = options.Value;
            _connectionString = settings.ConnectionString;
            _wishlistTableName = settings.WishlistTableName;
            _itemTableName = settings.ItemTableName;
        }
        public Task<WishlistItem> AddAsync(WishlistItem entity)
        {
            throw new NotImplementedException();
        }

        public Task<Wishlist> AddAsync(Wishlist entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(WishlistItem entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Wishlist entity)
        {
            throw new NotImplementedException();
        }

        public Task<WishlistItem> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Wishlist>> GetWishlistByOwnerAsync(string owner)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<WishlistItem>> ListAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(WishlistItem entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Wishlist entity)
        {
            throw new NotImplementedException();
        }

        Task<Wishlist> IAsyncRepository<Wishlist>.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<IReadOnlyList<Wishlist>> IAsyncRepository<Wishlist>.ListAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
