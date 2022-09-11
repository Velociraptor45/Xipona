using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Reasons;

public class RecipeNotFoundReason : IReason
{
    public RecipeNotFoundReason(RecipeId recipeId)
    {
        Message = $"Recipe {recipeId.Value} not found";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.RecipeNotFound;
}