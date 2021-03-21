namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingListStore : IShoppingListStore
    {
        public ShoppingListStore(ShoppingListStoreId id, string name, bool isDeleted)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
        }

        public ShoppingListStoreId Id { get; }
        public bool IsDeleted { get; }

        public string Name { get; private set; }

        public void ChangeName(string name)
        {
            Name = name;
        }
    }
}