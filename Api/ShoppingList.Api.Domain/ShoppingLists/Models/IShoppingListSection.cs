using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public interface IShoppingListSection
    {
        ShoppingListSectionId Id { get; }
        public IReadOnlyCollection<IShoppingListItem> Items { get; }

        IShoppingListSection AddItem(IShoppingListItem item);

        IShoppingListSection ChangeItemQuantity(ShoppingListItemId itemId, float quantity);

        bool ContainsItem(ShoppingListItemId itemId);

        IShoppingListSection PutItemInBasket(ShoppingListItemId itemId);

        IShoppingListSection RemoveItemFromBasket(ShoppingListItemId itemId);

        IShoppingListSection RemoveItem(ShoppingListItemId itemId);

        IShoppingListSection RemoveItemsInBasket();

        IShoppingListSection RemoveItemsNotInBasket();
    }
}