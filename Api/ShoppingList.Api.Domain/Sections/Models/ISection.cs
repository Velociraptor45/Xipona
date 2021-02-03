using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Sections.Models
{
    public interface ISection
    {
        SectionId Id { get; }
        string Name { get; }
        int SortingIndex { get; }

        public IReadOnlyCollection<IShoppingListItem> ShoppingListItems { get; }
        bool IsDefaultSection { get; }

        void AddItem(IShoppingListItem item);

        void ChangeItemQuantity(ShoppingListItemId itemId, float quantity);

        bool ContainsItem(ShoppingListItemId id);

        void PutItemInBasket(ShoppingListItemId itemId);

        void RemoveItemFromBasket(ShoppingListItemId itemId);

        void RemoveItem(ShoppingListItemId id);
        void RemoveAllItemsInBasket();
        void RemoveAllItemsNotInBasket();
    }
}