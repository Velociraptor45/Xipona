using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Recipes.Models;

namespace Xipona.Api.Domain.Recipes.Services.Queries;
public record IngredientReadModel(IngredientId Id, string Name, ItemCategoryId ItemCategoryId,
    IngredientQuantityType QuantityType, IngredientQuantity Quantity,
    IngredientShoppingListProperties? ShoppingListProperties);