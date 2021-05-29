using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ItemCategories.Fixtures
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