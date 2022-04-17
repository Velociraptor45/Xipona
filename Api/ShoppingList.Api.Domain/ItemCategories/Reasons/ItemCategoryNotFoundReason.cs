using ProjectHermes.ShoppingList.Api.Domain.Common.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Reasons;

public class ItemCategoryNotFoundReason : IReason
{
    public ItemCategoryNotFoundReason(ItemCategoryId id)
    {
        Message = $"Item category {id.Value} not found.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemCategoryNotFound;
}