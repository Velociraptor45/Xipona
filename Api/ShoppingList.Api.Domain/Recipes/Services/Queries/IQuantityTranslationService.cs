using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

public interface IQuantityTranslationService
{
    (QuantityType QuantityType, Quantity Quantity) NormalizeForOneServing(NumberOfServings definedNumberOfServings,
        IngredientQuantityType ingredientQuantityType, IngredientQuantity ingredientQuantity, ItemQuantity itemQuantity);
}