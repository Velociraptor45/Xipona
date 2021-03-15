using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public interface IShoppingListStoreFactory
    {
        IShoppingListStore Create(ShoppingListStoreId id, string name, bool isDeleted);
        IShoppingListStore Create(IStore store);
    }
}