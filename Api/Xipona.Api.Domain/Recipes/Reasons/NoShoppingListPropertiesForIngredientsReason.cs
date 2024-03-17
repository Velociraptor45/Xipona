using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;

public class CannotChangeStoreOfIngredientWithoutShoppingListPropertiesReason : IReason
{
    public CannotChangeStoreOfIngredientWithoutShoppingListPropertiesReason(IngredientId ingredientId)
    {
        Message = $"Cannot change store of ingredient ({ingredientId.Value}) because it has no shopping list properties";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotChangeStoreOfIngredientWithoutShoppingListProperties;
}