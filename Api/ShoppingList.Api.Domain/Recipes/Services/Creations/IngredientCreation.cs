using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

public class IngredientCreation
{
    public IngredientCreation(ItemCategoryId itemCategoryId, IngredientQuantityType quantityType, IngredientQuantity quantity)
    {
        ItemCategoryId = itemCategoryId;
        QuantityType = quantityType;
        Quantity = quantity;
    }

    public ItemCategoryId ItemCategoryId { get; }
    public IngredientQuantityType QuantityType { get; }
    public IngredientQuantity Quantity { get; }
}