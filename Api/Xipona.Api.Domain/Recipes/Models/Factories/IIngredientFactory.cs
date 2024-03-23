using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Creations;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models.Factories;

public interface IIngredientFactory
{
    Task<IIngredient> CreateNewAsync(IngredientCreation creation);

    Task<IIngredient> CreateNewAsync(ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity, IngredientShoppingListProperties? shoppingListProperties);

    IIngredient Create(IngredientId id, ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity, IngredientShoppingListProperties? shoppingListProperties);
}