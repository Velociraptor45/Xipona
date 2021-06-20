using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Exceptions
{
    public class ApiProcessingException : Exception
    {
        public ApiProcessingException(string message) : base(message)
        {
        }

        public ApiProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}