using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models;

public record IngredientShoppingListProperties(ItemId DefaultItemId, ItemTypeId? DefaultItemTypeId,
    StoreId DefaultStoreId, bool AddToShoppingListByDefault);