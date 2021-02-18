using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
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

        public StoreItemMock Create(StoreItemDefinition definition)
        {
            return CreateMany(definition.ToMonoList()).First();
        }

        public IEnumerable<StoreItemMock> CreateMany(IEnumerable<StoreItemDefinition> definitions)
        {
            return storeItemFixture.CreateMany(definitions).Select(item => new StoreItemMock(item));
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