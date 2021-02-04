using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommand : ICommand<bool>
    {
        public AddItemToShoppingListCommand(ShoppingListId shoppingListId, ShoppingListItemId shoppingListItemId,
            ShoppingListSectionId sectionId, float quantity)
        {
            ShoppingListId = shoppingListId ?? throw new ArgumentNullException(nameof(shoppingListId));
            ShoppingListItemId = shoppingListItemId ?? throw new ArgumentNullException(nameof(shoppingListItemId));
            SectionId = sectionId ?? throw new ArgumentNullException(nameof(sectionId));
            Quantity = quantity;
        }

        public ShoppingListId ShoppingListId { get; }
        public ShoppingListItemId ShoppingListItemId { get; }
        public ShoppingListSectionId SectionId { get; }
        public float Quantity { get; }
    }
}