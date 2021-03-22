using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommand : ICommand<bool>
    {
        public AddItemToShoppingListCommand(ShoppingListId shoppingListId, ItemId shoppingListItemId,
            SectionId sectionId, float quantity)
        {
            ShoppingListId = shoppingListId ?? throw new ArgumentNullException(nameof(shoppingListId));
            ShoppingListItemId = shoppingListItemId ?? throw new ArgumentNullException(nameof(shoppingListItemId));
            SectionId = sectionId;
            Quantity = quantity;
        }

        public ShoppingListId ShoppingListId { get; }
        public ItemId ShoppingListItemId { get; }
        public SectionId SectionId { get; }
        public float Quantity { get; }
    }
}