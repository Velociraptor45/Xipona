using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.Shared;

namespace ShoppingList.Api.Domain.TestKit.ItemCategories.Fixtures
{
    public class ItemCategoryFixture
    {
        private readonly CommonFixture commonFixture;

        public ItemCategoryFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public IItemCategory GetItemCategory(ItemCategoryId id = null, string name = null, bool? isDeleted = null)
        {
            var fixture = commonFixture.GetNewFixture();

            if (id != null)
                fixture.ConstructorArgumentFor<ItemCategory, ItemCategoryId>("id", id);
            if (name != null)
                fixture.ConstructorArgumentFor<ItemCategory, string>("name", name);
            if (isDeleted.HasValue)
                fixture.ConstructorArgumentFor<ItemCategory, bool>("isDeleted", isDeleted.Value);

            return fixture.Create<ItemCategory>();
        }
    }
}