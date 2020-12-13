namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public enum ErrorReasonCode
    {
        ItemNotFound = 0,
        ActualIdRequired = 1,
        InvalidItemQuantity = 2,
        ItemAlreadyOnShoppingList = 3,
        ItemAtStoreNotAvailable = 4,
        ItemCategoryNotFound = 5,
        ItemNotTemporary = 6,
        ItemNotOnShoppingList = 7,
        ManufacturerNotFound = 8,
        ShoppingListAlreadyExists = 9,
        ShoppingListAlreadyFinished = 10,
        ShoppingListNotFound = 11,
        StoreNotFound = 12,
        TemporaryItemNotModifyable = 13,
        TemporaryItemNotUpadateable = 14,
    }
}