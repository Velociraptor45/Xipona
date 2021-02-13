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
        private Dictionary<ShoppingListSectionId, IShoppingListSection> sections;

        public ShoppingList(ShoppingListId id, IStore store, IEnumerable<IShoppingListSection> sections, DateTime? completionDate)
        {
            Id = id;
            Store = store;
            this.sections = sections.ToDictionary(s => s.Id);
            CompletionDate = completionDate;
        }

        public ShoppingListId Id { get; }
        public IStore Store { get; }
        public IReadOnlyCollection<IShoppingListItem> Items => Sections.SelectMany(s => s.ShoppingListItems).ToList().AsReadOnly();
        public DateTime? CompletionDate { get; private set; }
        public IReadOnlyCollection<IShoppingListSection> Sections => sections.Values.ToList().AsReadOnly();

        public void AddItem(IShoppingListItem item, ShoppingListSectionId sectionId)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (!item.Id.IsActualId)
                throw new DomainException(new ActualIdRequiredReason(item.Id));

            var existingItem = Items.FirstOrDefault(it => it.Id == item.Id);
            if (existingItem != null)
                throw new DomainException(new ItemAlreadyOnShoppingListReason(item.Id, Id));

            IShoppingListSection section;
            if (sectionId == null)
            {
                section = Sections.SingleOrDefault(s => s.IsDefaultSection);
                if (section == null)
                    throw new DomainException(new NoDefaultSectionSpecifiedReason(Store.Id));
            }
            else
            {
                if (!sections.TryGetValue(sectionId, out section))
                    throw new DomainException(new SectionNotPartOfStoreReason(sectionId, Store.Id));
            }

            section.AddItem(item);
        }

        public void RemoveItem(ShoppingListItemId id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (!id.IsActualId)
                throw new DomainException(new ActualIdRequiredReason(id));

            IShoppingListSection section = sections.Values.FirstOrDefault(s => s.ContainsItem(id));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, id));

            section.RemoveItem(id);
        }

        public void PutItemInBasket(ShoppingListItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));
            if (!itemId.IsActualId)
                throw new DomainException(new ActualIdRequiredReason(itemId));

            IShoppingListSection section = sections.Values.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

            section.PutItemInBasket(itemId);
        }

        public void RemoveFromBasket(ShoppingListItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));
            if (!itemId.IsActualId)
                throw new DomainException(new ActualIdRequiredReason(itemId));

            IShoppingListSection section = sections.Values.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

            section.RemoveItemFromBasket(itemId);
        }

        public void ChangeItemQuantity(ShoppingListItemId itemId, float quantity)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));
            if (!itemId.IsActualId)
                throw new DomainException(new ActualIdRequiredReason(itemId));
            if (quantity <= 0f)
                throw new DomainException(new InvalidItemQuantityReason(quantity));

            IShoppingListSection section = sections.Values.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

            section.ChangeItemQuantity(itemId, quantity);
        }

        public void SetCompletionDate(DateTime completionDate)
        {
            CompletionDate = completionDate;
        }

        public IEnumerable<IShoppingListSection> GetSectionsWithItemsNotInBasket()
        {
            foreach (var section in sections.Values)
            {
                section.RemoveAllItemsInBasket();
                yield return section;
            }
        }

        public void RemoveAllItemsNotInBasket()
        {
            foreach (var section in sections.Values)
            {
                section.RemoveAllItemsInBasket();
            }
        }
    }
}