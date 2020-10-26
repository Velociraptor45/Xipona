using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Domain.Exceptions
{
    public class StoreNotFoundException : Exception
    {
        public StoreNotFoundException(StoreId id)
            : base($"Store {id.Value} not found")
        {
        }

        public StoreNotFoundException(string message) : base(message)
        {
        }

        public StoreNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}