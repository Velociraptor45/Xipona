﻿using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class ShoppingListAlreadyExistsReason : IReason
{
    public ShoppingListAlreadyExistsReason(StoreId storeId)
    {
        Message = $"There's already an active shoppingList for store {storeId.Value}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ShoppingListAlreadyExists;
}