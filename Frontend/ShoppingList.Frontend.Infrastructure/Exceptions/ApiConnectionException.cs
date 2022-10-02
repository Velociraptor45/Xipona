using System;
using System.Runtime.Serialization;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Exceptions
{
    [Serializable]
    public class ApiConnectionException : Exception
    {
        public ApiConnectionException(string message) : base(message)
        {
        }

        public ApiConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApiConnectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}