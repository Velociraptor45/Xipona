using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Exceptions
{
    public class ShoppingListAlreadyFinishedException : Exception
    {
        public ShoppingListAlreadyFinishedException(ShoppingListId id)
            : base($"Shopping list {id.Value} is already finished.")
        {
        }

        public ShoppingListAlreadyFinishedException(string message) : base(message)
        {
        }

        public ShoppingListAlreadyFinishedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}