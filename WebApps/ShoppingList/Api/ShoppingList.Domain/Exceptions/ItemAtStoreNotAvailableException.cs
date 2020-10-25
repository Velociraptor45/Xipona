using ShoppingList.Domain.Models;
using System;

namespace ShoppingList.Domain.Exceptions
{
    public class ItemAtStoreNotAvailableException : Exception
    {
        public ItemAtStoreNotAvailableException(StoreItemId itemId, StoreId storeId)
            : base($"Item {itemId.Value} not available at store {storeId.Value}")
        {
        }

        public ItemAtStoreNotAvailableException(string message) : base(message)
        {
        }

        public ItemAtStoreNotAvailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}