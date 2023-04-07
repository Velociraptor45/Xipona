using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Reasons;

public class InvalidRecipeTagIdsReason : IReason
{
    public InvalidRecipeTagIdsReason(IEnumerable<RecipeTagId> invalidRecipeTagIds)
    {
        var tags = string.Join(", ", invalidRecipeTagIds.Select(t => t.Value));
        Message = $"The following recipe tag ids do not exist: {tags}";
        ErrorCode = ErrorReasonCode.InvalidRecipeTagIds;
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode { get; }
}