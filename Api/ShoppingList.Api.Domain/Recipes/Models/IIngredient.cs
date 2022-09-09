using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public interface IIngredient
{
    IngredientId Id { get; }
    ItemCategoryId ItemCategoryId { get; }
    IngredientQuantityType QuantityType { get; }
    IngredientQuantity Quantity { get; }
    Task<IIngredient> ModifyAsync(IngredientModification modification, IValidator validator);
}