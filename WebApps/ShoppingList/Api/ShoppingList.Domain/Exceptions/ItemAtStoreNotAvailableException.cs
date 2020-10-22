using System;

namespace ShoppingList.Domain.Exceptions
{
    public class ItemAtStoreNotAvailableException : Exception
    {
        public ItemAtStoreNotAvailableException()
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