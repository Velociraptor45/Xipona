using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures
{
    public class ItemCategoryMockFixture
    {
        private readonly CommonFixture commonFixture;
        private readonly ItemCategoryFixture itemCategoryFixture;

        public ItemCategoryMockFixture(CommonFixture commonFixture, ItemCategoryFixture itemCategoryFixture)
        {
            this.commonFixture = commonFixture;
            this.itemCategoryFixture = itemCategoryFixture;
        }

        public ItemCategoryMock Create()
        {
            return CreateMany(1).First();
        }

        public IEnumerable<ItemCategoryMock> CreateMany(int amount)
        {
            var uniqueIds = commonFixture.NextUniqueInts(amount);

            foreach (var id in uniqueIds)
            {
                var itemCategory = itemCategoryFixture.GetItemCategory(new ItemCategoryId(id));
                yield return new ItemCategoryMock(itemCategory);
            }
        }
    }
}