using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels
{
    public class ShoppingListStoreReadModel
    {
        public ShoppingListStoreReadModel(ShoppingListStoreId id, string name, bool isDeleted)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
        }

        public ShoppingListStoreId Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
    }
}