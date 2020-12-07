using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Exceptions
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