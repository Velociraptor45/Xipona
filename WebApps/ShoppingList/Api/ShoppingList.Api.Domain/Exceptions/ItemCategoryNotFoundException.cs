using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Domain.Exceptions
{
    public class ItemCategoryNotFoundException : Exception
    {
        public ItemCategoryNotFoundException(ItemCategoryId id)
            : base($"Item category {id.Value} not found.")
        {
        }

        public ItemCategoryNotFoundException(string message) : base(message)
        {
        }

        public ItemCategoryNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}