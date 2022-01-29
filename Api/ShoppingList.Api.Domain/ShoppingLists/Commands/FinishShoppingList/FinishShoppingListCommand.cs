using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.FinishShoppingList
{
    public class FinishShoppingListCommand : ICommand<bool>
    {
        public FinishShoppingListCommand(ShoppingListId shoppingListId, DateTime completionDate)
        {
            ShoppingListId = shoppingListId;
            CompletionDate = completionDate;
        }

        public ShoppingListId ShoppingListId { get; }
        public DateTime CompletionDate { get; }
    }
}