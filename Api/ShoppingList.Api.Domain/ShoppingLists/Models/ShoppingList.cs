using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingList : IShoppingList
    {
        private IEnumerable<IShoppingListSection> sections;

        public ShoppingList(ShoppingListId id, IStore store, IEnumerable<IShoppingListSection> sections, DateTime? completionDate)
        {
            Id = id;
            Store = store;
            this.sections = sections;
            CompletionDate = completionDate;
        }

        public ShoppingListId Id { get; }
        public IStore Store { get; }
        public IReadOnlyCollection<IShoppingListItem> Items => Sections.SelectMany(s => s.ShoppingListItems).ToList().AsReadOnly();
        public DateTime? CompletionDate { get; private set; }
        public IReadOnlyCollection<IShoppingListSection> Sections => sections.ToList().AsReadOnly();

        public void AddItem(IShoppingListItem item, ShoppingListSectionId sectionId)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (!item.Id.IsActualId)
                throw new DomainException(new ActualIdRequiredReason(item.Id));

            var existingItem = Items.FirstOrDefault(it => it.Id == item.Id);
            if (existingItem != null)
                throw new DomainException(new ItemAlreadyOnShoppingListReason(item.Id, Id));

            var sectionList = sections.ToList();

            IShoppingListSection section;
            if (sectionId == null)
            {
                section = sectionList.SingleOrDefault(s => s.IsDefaultSection);
                if (section == null)
                    throw new DomainException(new NoDefaultSectionSpecifiedReason(Store.Id));
            }
            else
            {
                section = sectionList.FirstOrDefault(s => s.Id == sectionId);
                if (section == null)
                    throw new DomainException(new SectionNotPartOfStoreReason(sectionId, Store.Id));
            }

            section.AddItem(item);
            sections = sectionList;
        }

        public void RemoveItem(ShoppingListItemId id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (!id.IsActualId)
                throw new DomainException(new ActualIdRequiredReason(id));

            var sectionList = sections.ToList();
            IShoppingListSection section = sectionList.FirstOrDefault(s => s.ContainsItem(id));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, id));

            section.RemoveItem(id);

            sections = sectionList;
        }

        public void PutItemInBasket(ShoppingListItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));
            if (!itemId.IsActualId)
                throw new DomainException(new ActualIdRequiredReason(itemId));

            var sectionList = sections.ToList();
            IShoppingListSection section = sectionList.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

            section.PutItemInBasket(itemId);

            sections = sectionList;
        }

        public void RemoveFromBasket(ShoppingListItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));
            if (!itemId.IsActualId)
                throw new DomainException(new ActualIdRequiredReason(itemId));

            var sectionList = sections.ToList();
            IShoppingListSection section = sectionList.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

            section.RemoveItemFromBasket(itemId);

            sections = sectionList;
        }

        public void ChangeItemQuantity(ShoppingListItemId itemId, float quantity)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));
            if (!itemId.IsActualId)
                throw new DomainException(new ActualIdRequiredReason(itemId));
            if (quantity <= 0f)
                throw new DomainException(new InvalidItemQuantityReason(quantity));

            var sectionList = sections.ToList();
            IShoppingListSection section = sectionList.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

            section.ChangeItemQuantity(itemId, quantity);

            sections = sectionList;
        }

        /// <summary>
        /// Finishes the current shopping list and returns a new shopping list with all items that were not in the
        /// basket on it
        /// </summary>
        /// <returns></returns>
        public void SetCompletionDate(DateTime completionDate)
        {
            CompletionDate = completionDate;
        }

        public IEnumerable<IShoppingListSection> GetSectionsWithItemsNotInBasket()
        {
            var sectionList = sections.ToList();
            foreach (var section in sectionList)
            {
                section.RemoveAllItemsInBasket();
                yield return section;
            }
        }

        public void RemoveAllItemsNotInBasket()
        {
            var updatedSections = new List<IShoppingListSection>();

            var sectionList = sections.ToList();
            foreach (var section in sectionList)
            {
                section.RemoveAllItemsInBasket();
                updatedSections.Add(section);
            }

            sections = updatedSections;
        }
    }
}