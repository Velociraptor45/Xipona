using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures
{
    public class StoreItemMockFixture
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreItemFixture storeItemFixture;

        public StoreItemMockFixture(CommonFixture commonFixture, StoreItemFixture storeItemFixture)
        {
            this.commonFixture = commonFixture;
            this.storeItemFixture = storeItemFixture;
        }

        public StoreItemMock Create()
        {
            return CreateMany(1).First();
        }

        public IEnumerable<StoreItemMock> CreateMany(int amount)
        {
            var uniqueIds = commonFixture.NextUniqueInts(amount);

            foreach (var id in uniqueIds)
            {
                var storeItem = storeItemFixture.GetStoreItem(new StoreItemId(id));
                yield return new StoreItemMock(storeItem);
            }
        }
    }
}