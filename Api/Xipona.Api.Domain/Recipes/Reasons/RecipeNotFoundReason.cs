using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Recipes.Models;

namespace Xipona.Api.Domain.Recipes.Reasons;

public class RecipeNotFoundReason : IReason
{
    public RecipeNotFoundReason(RecipeId recipeId)
    {
        Message = $"Recipe {recipeId.Value} not found";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.RecipeNotFound;
}