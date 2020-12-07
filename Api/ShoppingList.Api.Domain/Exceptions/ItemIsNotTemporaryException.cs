using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Exceptions
{
    public class ItemIsNotTemporaryException : Exception
    {
        public ItemIsNotTemporaryException(StoreItemId id)
            : base($"Item {id} is not temporary.")
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