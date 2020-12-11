using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Exceptions
{
    public class ShoppingListNotFoundException : Exception
    {
        public ShoppingListNotFoundException(ShoppingListId id)
            : base($"Shopping list {id.Value} not found")
        {
        }

        public ShoppingListNotFoundException(string message) : base(message)
        {
        }

        public ShoppingListNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}