using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;

public class ShoppingListEntityBuilder : TestBuilderBase<ShoppingList>
{
    public ShoppingListEntityBuilder()
    {
        WithItemsOnList(new ItemsOnListEntityBuilder().CreateMany(3).ToList());
    }

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

    public ShoppingListEntityBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        FillPropertyWith(p => p.CreatedAt, createdAt);
        return this;
    }

    public ShoppingListEntityBuilder WithItemsOnList(ICollection<ItemsOnList> itemsOnList)
    {
        FillPropertyWith(p => p.ItemsOnList, itemsOnList);
        return this;
    }

    public ShoppingListEntityBuilder WithEmptyItemsOnList()
    {
        return WithItemsOnList(new List<ItemsOnList>());
    }
}