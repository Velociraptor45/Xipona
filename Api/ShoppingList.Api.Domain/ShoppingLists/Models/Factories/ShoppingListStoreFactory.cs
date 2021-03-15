using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public class ShoppingListStoreFactory : IShoppingListStoreFactory
    {
        public IShoppingListStore Create(ShoppingListStoreId id, string name, bool isDeleted)
        {
            return new ShoppingListStore(
                id,
                name,
                isDeleted);
        }

        public IShoppingListStore Create(IStore store)
        {
            return new ShoppingListStore(
                store.Id.AsShoppingListStoreId(),
                store.Name,
                store.IsDeleted);
        }
    }
}