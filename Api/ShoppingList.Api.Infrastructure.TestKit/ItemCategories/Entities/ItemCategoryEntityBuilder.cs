using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.ItemCategories.Entities;

public class ItemCategoryEntityBuilder : TestBuilder<ItemCategory>
{
    public ItemCategoryEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(e => e.Id, id);
        return this;
    }
}