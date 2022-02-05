using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class ItemCategoryNotFoundReason : IReason
{
    public ItemCategoryNotFoundReason(ItemCategoryId id)
    {
        Message = $"Item category {id.Value} not found.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemCategoryNotFound;
}