using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
public record IngredientReadModel(IngredientId Id, string Name, ItemCategoryId ItemCategoryId,
    IngredientQuantityType QuantityType, IngredientQuantity Quantity,
    IngredientShoppingListProperties? ShoppingListProperties);