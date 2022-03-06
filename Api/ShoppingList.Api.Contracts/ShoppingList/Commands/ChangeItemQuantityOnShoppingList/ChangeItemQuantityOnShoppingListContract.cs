using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.ChangeItemQuantityOnShoppingList
{
    public class ChangeItemQuantityOnShoppingListContract
    {
        public int ShoppingListId { get; set; }
        public ItemIdContract ItemId { get; set; }
        public float Quantity { get; set; }
        public Guid? ItemTypeId { get; set; }
    }
}