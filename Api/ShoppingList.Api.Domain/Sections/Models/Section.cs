using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Sections.Models
{
    public class Section : ISection
    {
        private IEnumerable<IShoppingListItem> shoppingListItems;

        public Section(SectionId id, string name, IEnumerable<IShoppingListItem> shoppingListItems, int sortingIndex,
            bool isDefaultSection)
        {
            Id = id;
            Name = name;
            this.shoppingListItems = shoppingListItems;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
        }

        public SectionId Id { get; }
        public string Name { get; }
        public int SortingIndex { get; }
        public bool IsDefaultSection { get; }

        public IReadOnlyCollection<IShoppingListItem> ShoppingListItems => shoppingListItems.ToList().AsReadOnly();

        public void RemoveItem(ShoppingListItemId itemId)
        {
            var items = shoppingListItems.ToList();
            var updatedItems = items.Where(i => i.Id != itemId).ToList();

            if (updatedItems.Count == items.Count)
                throw new DomainException(new ItemNotInSectionReason(itemId, Id));

            shoppingListItems = updatedItems;
        }

        public bool ContainsItem(ShoppingListItemId id)
        {
            return shoppingListItems.Any(item => item.Id == id);
        }

        public void AddItem(IShoppingListItem item)
        {
            var items = shoppingListItems.ToList();

            if (items.Any(i => i.Id == item.Id))
                throw new DomainException(new ItemAlreadyInSectionReason(item.Id, Id));

            items.Add(item);
            shoppingListItems = items;
        }

        public void PutItemInBasket(ShoppingListItemId itemId)
        {
            var items = shoppingListItems.ToList();
            var item = items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
                throw new DomainException(new ItemNotInSectionReason(itemId, Id));

            item.PutInBasket();

            shoppingListItems = items;
        }

        public void RemoveItemFromBasket(ShoppingListItemId itemId)
        {
            var items = shoppingListItems.ToList();
            var item = items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
                throw new DomainException(new ItemNotInSectionReason(itemId, Id));

            item.RemoveFromBasket();

            shoppingListItems = items;
        }

        public void ChangeItemQuantity(ShoppingListItemId itemId, float quantity)
        {
            var items = shoppingListItems.ToList();
            var item = items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
                throw new DomainException(new ItemNotInSectionReason(itemId, Id));

            item.ChangeQuantity(quantity);

            shoppingListItems = items;
        }

        public void RemoveAllItemsInBasket()
        {
            var items = shoppingListItems.ToList();
            shoppingListItems = items.Where(i => !i.IsInBasket);
        }

        public void RemoveAllItemsNotInBasket()
        {
            var items = shoppingListItems.ToList();
            shoppingListItems = items.Where(i => i.IsInBasket);
        }
    }
}