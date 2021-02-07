using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public interface IShoppingList
    {
        public ShoppingListId Id { get; }
        public IStore Store { get; }
        public IReadOnlyCollection<IShoppingListItem> Items { get; }
        public DateTime? CompletionDate { get; }
        IReadOnlyCollection<IShoppingListSection> Sections { get; }

        void AddItem(IShoppingListItem item, ShoppingListSectionId sectionId);

        void RemoveItem(ShoppingListItemId id);

        void PutItemInBasket(ShoppingListItemId itemId);

        void RemoveFromBasket(ShoppingListItemId itemId);

        void ChangeItemQuantity(ShoppingListItemId itemId, float quantity);

        void SetCompletionDate(DateTime completionDate);

        IEnumerable<IShoppingListSection> GetSectionsWithItemsNotInBasket();

        void RemoveAllItemsNotInBasket();
    }
}