using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Exceptions
{
    public class ItemNotOnShoppingListException : Exception
    {
        public ShoppingListId ShoppingListId { get; }
        public ShoppingListItemId ShoppingListItemId { get; }

        public ItemNotOnShoppingListException()
        {
        }

        public ItemNotOnShoppingListException(ShoppingListId shoppingListId, ShoppingListItemId shoppingListItemId)
        {
            ShoppingListId = shoppingListId;
            ShoppingListItemId = shoppingListItemId;
        }

        public ItemNotOnShoppingListException(string message) : base(message)
        {
        }

        public ItemNotOnShoppingListException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}