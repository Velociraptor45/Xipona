using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public interface IIngredient
{
    IngredientId Id { get; }
    ItemCategoryId ItemCategoryId { get; }
    IngredientQuantityType QuantityType { get; }
    IngredientQuantity Quantity { get; }
}