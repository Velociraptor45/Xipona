using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.ShoppingLists.Entities;

public class ShoppingListBuilder : TestBuilderBase<Infrastructure.ShoppingLists.Entities.ShoppingList>
{
    public ShoppingListBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public ShoppingListBuilder WithCompletionDate(DateTimeOffset? completionDate)
    {
        FillPropertyWith(p => p.CompletionDate, completionDate);
        return this;
    }

    public ShoppingListBuilder WithoutCompletionDate()
    {
        return WithCompletionDate(null);
    }

    public ShoppingListBuilder WithStoreId(Guid storeId)
    {
        FillPropertyWith(p => p.StoreId, storeId);
        return this;
    }

    public ShoppingListBuilder WithItemsOnList(ICollection<ItemsOnList> itemsOnList)
    {
        FillPropertyWith(p => p.ItemsOnList, itemsOnList);
        return this;
    }
}