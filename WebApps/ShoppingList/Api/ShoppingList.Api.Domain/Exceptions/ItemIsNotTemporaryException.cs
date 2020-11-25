using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Domain.Exceptions
{
    public class ItemIsNotTemporaryException : Exception
    {
        public ItemIsNotTemporaryException(StoreItemId id)
            : base($"Item {id.Value} is not temporary.")
        {
        }

        public ItemIsNotTemporaryException(string message) : base(message)
        {
        }

        public ItemIsNotTemporaryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}