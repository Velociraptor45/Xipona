using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Domain.Exceptions
{
    public class TemporaryItemNotUpdateableException : Exception
    {
        public TemporaryItemNotUpdateableException(StoreItemId id)
            : base($"Item {id} is temporary and thus cannot be updated.")
        {
        }

        public TemporaryItemNotUpdateableException(string message) : base(message)
        {
        }

        public TemporaryItemNotUpdateableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}