using Azure;
using Azure.Data.Tables;
using ItsyBitseList.Core.Constants;
using ItsyBitseList.Core.Interfaces.Persistence;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ItsyBitseList.Infrastructure.Persistence
{
    public static class Extensions
    {

        public static Wishlist AsDomainObject(this WishlistEntity entity)
        {
            var result = JsonConvert.DeserializeObject<WishlistState>(entity.Wishlist);

            return  Wishlist.CreateWith(result);
        }
    }

    public class WishlistEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Wishlist { get; set; }
        public WishlistEntity(Wishlist wishlist)
        {
            PartitionKey = wishlist.Owner;
            RowKey = wishlist.Id.ToString();
            Wishlist = JsonConvert.SerializeObject(wishlist.DataState);
        }
        public WishlistEntity() { }
        public WishlistEntity SetWishlist(Wishlist wishlist)
        {
            Wishlist = JsonConvert.SerializeObject(wishlist.DataState);
            return this;
        }
    }

    public class AzureTableRepository : IWishlistRepository, IAsyncRepository<Wishlist>
    {
        private TableClient _tableClient;
        private string _wishlistTableName;
        private string _connectionString;

        public AzureTableRepository(IOptions<StorageSettings> options)
        {
            var settings = options.Value;
            _connectionString = settings.ConnectionString;
            _wishlistTableName = settings.WishlistTableName;
            var serviceClient = new TableServiceClient(_connectionString);
            _tableClient = serviceClient.GetTableClient(_wishlistTableName);

            _tableClient.CreateIfNotExists();
        }


        public async Task<Wishlist> AddAsync(Wishlist entity)
        {
            await _tableClient.AddEntityAsync(new WishlistEntity(entity));
            return entity;
        }


        public async Task DeleteAsync(Wishlist entity)
        {
            await _tableClient.DeleteEntityAsync(entity.Owner, entity.Id.ToString());
        }

        public async Task<Wishlist> GetByIdAsync(Guid id)
        {
            var wishlist = await _tableClient.GetEntityIfExistsAsync<WishlistEntity>(Wishlist.DefaultOwner, id.ToString());

            return wishlist?.Value.AsDomainObject() ?? throw new InvalidOperationException(ErrorMessages.WishlistNotFound);
        }


        public Task<IEnumerable<Wishlist>> GetWishlistByOwnerAsync(string owner)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Wishlist>> ListAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Wishlist entity)
        {
            var existingEntity = await _tableClient.GetEntityIfExistsAsync<WishlistEntity>(entity.Owner, entity.Id.ToString());
            if (existingEntity == null)
            {
                throw new InvalidOperationException(ErrorMessages.WishlistNotFound);
            }
            else
            {
                await _tableClient.UpdateEntityAsync(existingEntity.Value.SetWishlist(entity), existingEntity.Value.ETag);
            }
        }

    }
}
