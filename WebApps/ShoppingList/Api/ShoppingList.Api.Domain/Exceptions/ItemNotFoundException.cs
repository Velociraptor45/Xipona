using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Domain.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(StoreItemId id)
            : base($"Item {id} not found.")
        {
        }

        public ItemNotFoundException(string message) : base(message)
        {
        }

        public ItemNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}