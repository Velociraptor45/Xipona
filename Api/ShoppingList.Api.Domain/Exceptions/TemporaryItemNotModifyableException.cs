using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Exceptions
{
    public class TemporaryItemNotModifyableException : Exception
    {
        public TemporaryItemNotModifyableException(StoreItemId id)
            : base($"Item {id} is temporary and thus cannot be modified.")
        {
        }

        public TemporaryItemNotModifyableException(string message) : base(message)
        {
        }

        public TemporaryItemNotModifyableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}