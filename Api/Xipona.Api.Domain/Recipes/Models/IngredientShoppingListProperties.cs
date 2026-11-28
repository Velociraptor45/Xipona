using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Recipes.Models;

public record IngredientShoppingListProperties(ItemId DefaultItemId, ItemTypeId? DefaultItemTypeId,
    StoreId DefaultStoreId, bool AddToShoppingListByDefault);