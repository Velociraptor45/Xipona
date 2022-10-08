using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.ShoppingLists.Entities;

public class ShoppingListEntityBuilder : TestBuilderBase<Infrastructure.ShoppingLists.Entities.ShoppingList>
{
    public ShoppingListEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public ShoppingListEntityBuilder WithCompletionDate(DateTimeOffset? completionDate)
    {
        FillPropertyWith(p => p.CompletionDate, completionDate);
        return this;
    }

    public ShoppingListEntityBuilder WithoutCompletionDate()
    {
        return WithCompletionDate(null);
    }

    public ShoppingListEntityBuilder WithStoreId(Guid storeId)
    {
        FillPropertyWith(p => p.StoreId, storeId);
        return this;
    }

    public ShoppingListEntityBuilder WithItemsOnList(ICollection<ItemsOnList> itemsOnList)
    {
        FillPropertyWith(p => p.ItemsOnList, itemsOnList);
        return this;
    }
}