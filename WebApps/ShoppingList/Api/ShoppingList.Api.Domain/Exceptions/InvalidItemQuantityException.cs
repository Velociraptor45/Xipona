using System;

namespace ShoppingList.Api.Domain.Exceptions
{
    public class InvalidItemQuantityException : Exception
    {
        public InvalidItemQuantityException(float quantity)
            : base($"Item quantity must be greater than 0 but was {quantity}")
        {
        }

        public InvalidItemQuantityException(string message) : base(message)
        {
        }

        public InvalidItemQuantityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}