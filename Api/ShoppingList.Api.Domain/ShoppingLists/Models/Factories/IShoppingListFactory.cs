using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;

public interface IShoppingListFactory
{
    IShoppingList Create(ShoppingListId id, StoreId storeId, DateTimeOffset? completionDate,
        IEnumerable<IShoppingListSection> sections);

    IShoppingList CreateNew(IStore store);
}