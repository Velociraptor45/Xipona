using System;
using System.Runtime.Serialization;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Exceptions
{
    [Serializable]
    public class ApiProcessingException : Exception
    {
        public ApiProcessingException(string message) : base(message)
        {
        }

        public ApiProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApiProcessingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}