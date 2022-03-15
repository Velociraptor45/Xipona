using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Queries;

public class ShoppingListStoreReadModel
{
    public ShoppingListStoreReadModel(StoreId id, string name)
    {
        Id = id;
        Name = name;
    }

    public StoreId Id { get; }
    public string Name { get; }
}