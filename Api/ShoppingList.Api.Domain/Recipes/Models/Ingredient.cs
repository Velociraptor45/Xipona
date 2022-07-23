using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class Ingredient : IIngredient
{
    public Ingredient(IngredientId id, ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity)
    {
        Id = id;
        ItemCategoryId = itemCategoryId;
        QuantityType = quantityType;
        Quantity = quantity;
    }

    public IngredientId Id { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public IngredientQuantityType QuantityType { get; }
    public IngredientQuantity Quantity { get; }
}