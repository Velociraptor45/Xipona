using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommand : ICommand<bool>
    {
        public AddItemToShoppingListCommand(ShoppingListId shoppingListId, ItemId itemId,
            SectionId sectionId, float quantity)
        {
            ShoppingListId = shoppingListId ?? throw new ArgumentNullException(nameof(shoppingListId));
            ItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
            SectionId = sectionId;
            Quantity = quantity;
        }

        public ShoppingListId ShoppingListId { get; }
        public ItemId ItemId { get; }
        public SectionId SectionId { get; }
        public float Quantity { get; }
    }
}