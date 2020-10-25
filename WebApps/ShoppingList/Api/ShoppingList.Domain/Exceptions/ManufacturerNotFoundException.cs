using System;

namespace ShoppingList.Domain.Exceptions
{
    public class ManufacturerNotFoundException : Exception
    {
        public ManufacturerNotFoundException(string message) : base(message)
        {
        }

        public ManufacturerNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}