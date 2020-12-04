using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Exceptions
{
    public class RetryException : Exception
    {
        public RetryException()
        {
        }

        public RetryException(string message) : base(message)
        {
        }

        public RetryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}