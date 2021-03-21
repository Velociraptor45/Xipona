namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public interface IShoppingListStore
    {
        ShoppingListStoreId Id { get; }
        bool IsDeleted { get; }
        string Name { get; }

        void ChangeName(string name);
    }
}