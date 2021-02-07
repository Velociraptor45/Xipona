using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons
{
    public class NoDefaultSectionSpecifiedReason : IReason
    {
        public NoDefaultSectionSpecifiedReason(StoreId id)
        {
            Message = $"No default section for store {id}'s shopping list.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.NoDefaultSectionSpecified;
    }
}