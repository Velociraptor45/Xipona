using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Exceptions
{
    public class ActualIdRequiredException : Exception
    {
        public ActualIdRequiredException(StoreItemId id)
            : base($"Store item needs to have an actual id instead of offline id {id.Offline}")
        {
        }

        public ActualIdRequiredException(ShoppingListItemId id)
            : base($"Shopping list item needs to have an actual id instead of offline id {id.Offline}")
        {
        }

        public ActualIdRequiredException(string message) : base(message)
        {
        }

        public ActualIdRequiredException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}