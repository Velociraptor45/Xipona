﻿using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class ActualIdRequiredReason : IReason
    {
        public ActualIdRequiredReason(StoreItemId id)
        {
            Message = $"Store item needs to have an actual id instead of offline id {id.Offline}";
        }

        public ActualIdRequiredReason(ShoppingListItemId id)
        {
            Message = $"Shopping list item needs to have an actual id instead of offline id {id.Offline}";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.ActualIdRequired;
    }
}