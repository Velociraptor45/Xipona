using System;

namespace ShoppingList.Domain.Exceptions
{
    public class ItemCategoryNotFoundException : Exception
    {
        public ItemCategoryNotFoundException(string message) : base(message)
        {
        }

        public ItemCategoryNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}