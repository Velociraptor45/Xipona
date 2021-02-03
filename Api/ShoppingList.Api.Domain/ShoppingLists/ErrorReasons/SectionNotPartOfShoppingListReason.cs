using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons
{
    public class SectionNotPartOfShoppingListReason : IReason
    {
        public string Message => throw new NotImplementedException();

        public ErrorReasonCode ErrorCode => throw new NotImplementedException();
    }
}