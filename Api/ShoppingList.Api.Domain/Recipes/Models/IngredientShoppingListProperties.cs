using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public record IngredientShoppingListProperties(ItemId DefaultItemId, ItemTypeId? DefaultItemTypeId,
    StoreId DefaultStoreId, bool AddToShoppingListByDefault);