using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public interface IShoppingListSection
    {
        ShoppingListSectionId Id { get; }
        string Name { get; }
        int SortingIndex { get; }

        public IReadOnlyCollection<IShoppingListItem> ShoppingListItems { get; }
        bool IsDefaultSection { get; }

        void AddItem(IShoppingListItem item);

        void ChangeItemQuantity(ShoppingListItemId itemId, float quantity);

        bool ContainsItem(ShoppingListItemId itemId);

        void PutItemInBasket(ShoppingListItemId itemId);

        void RemoveItemFromBasket(ShoppingListItemId itemId);

        void RemoveItem(ShoppingListItemId itemId);

        void RemoveAllItemsInBasket();

        void RemoveAllItemsNotInBasket();
    }
}