using ShoppingList.Domain.Models;
using System;

namespace ShoppingList.Domain.Exceptions
{
    public class ShoppingListAlreadyExistsException : Exception
    {
        public ShoppingListAlreadyExistsException(StoreId storeId)
            : base($"There's already an active shoppingList for store {storeId.Value}")
        {
        }

        public ShoppingListAlreadyExistsException(string message) : base(message)
        {
        }

        public ShoppingListAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}