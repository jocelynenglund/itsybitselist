using ItsyBitseList.Core.WishlistCollectionAggregate;
using ItsyBitseList.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ItsyBitseList.Tests.Infrastructure.Persitence
{
    public class WishlistEntityTests
    {
        [Fact]
        public void CanDeserializeJson()
        {
            var wishlist = new Wishlist()
            {
                Id = Guid.Parse(Constants.TestGuid),
                Items = new List<WishlistItem>()
                {
                    new WishlistItem(Guid.Parse(Constants.TestGuid), Guid.Parse(Constants.TestGuid), "My Description",State.Promised, null)
                },
                Name = "My Wishlist",
                Owner = "me"
            };
            var entity = new WishlistEntity(wishlist);

            var parsedEntity = JsonSerializer.Deserialize<Wishlist>(entity.Wishlist);
            Assert.NotNull(parsedEntity);
        }
    }
}
