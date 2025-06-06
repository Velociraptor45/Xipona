using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Recipes.Models;

namespace Xipona.Api.Domain.Recipes.Services.Modifications;

public class IngredientModification
{
    public IngredientModification(IngredientId? id, ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity, IngredientShoppingListProperties? shoppingListProperties)
    {
        Id = id;
        ItemCategoryId = itemCategoryId;
        QuantityType = quantityType;
        Quantity = quantity;
        ShoppingListProperties = shoppingListProperties;
    }

    public IngredientId? Id { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public IngredientQuantityType QuantityType { get; }
    public IngredientQuantity Quantity { get; }
    public IngredientShoppingListProperties? ShoppingListProperties { get; }
}