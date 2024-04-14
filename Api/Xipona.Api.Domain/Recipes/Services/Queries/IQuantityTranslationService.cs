using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

public interface IQuantityTranslationService
{
    (QuantityType QuantityType, Quantity Quantity) NormalizeForOneServing(NumberOfServings definedNumberOfServings,
        IngredientQuantityType ingredientQuantityType, IngredientQuantity ingredientQuantity, ItemQuantity itemQuantity);
}