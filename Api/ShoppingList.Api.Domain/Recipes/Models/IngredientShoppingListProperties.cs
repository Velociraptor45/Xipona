using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class IngredientShoppingListProperties
{
    public IngredientShoppingListProperties(ItemId defaultItemId, ItemTypeId? defaultItemTypeId,
        StoreId defaultStoreId, bool addToShoppingListByDefault)
    {
        DefaultItemId = defaultItemId;
        DefaultItemTypeId = defaultItemTypeId;
        DefaultStoreId = defaultStoreId;
        AddToShoppingListByDefault = addToShoppingListByDefault;
    }

    public ItemId DefaultItemId { get; }
    public ItemTypeId? DefaultItemTypeId { get; }
    public StoreId DefaultStoreId { get; }
    public bool AddToShoppingListByDefault { get; }
}