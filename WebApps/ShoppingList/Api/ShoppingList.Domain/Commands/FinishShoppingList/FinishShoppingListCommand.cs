using ShoppingList.Domain.Models;
using System;

namespace ShoppingList.Domain.Commands.FinishShoppingList
{
    public class FinishShoppingListCommand : ICommand<bool>
    {
        public FinishShoppingListCommand(ShoppingListId shoppingListId, DateTime completionDate)
        {
            ShoppingListId = shoppingListId ?? throw new ArgumentNullException(nameof(shoppingListId));
            CompletionDate = completionDate;
        }

        public ShoppingListId ShoppingListId { get; }
        public DateTime CompletionDate { get; }
    }
}