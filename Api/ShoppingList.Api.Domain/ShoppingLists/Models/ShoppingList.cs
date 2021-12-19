using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingList : IShoppingList
    {
        private readonly Dictionary<SectionId, IShoppingListSection> _sections;

        public ShoppingList(ShoppingListId id, StoreId storeId, DateTime? completionDate,
            IEnumerable<IShoppingListSection> sections)
        {
            Id = id;
            StoreId = storeId;
            CompletionDate = completionDate;
            _sections = sections.ToDictionary(s => s.Id);
        }

        public ShoppingListId Id { get; }
        public StoreId StoreId { get; }
        public DateTime? CompletionDate { get; private set; }

        public IReadOnlyCollection<IShoppingListSection> Sections => _sections.Values.ToList().AsReadOnly();
        public IReadOnlyCollection<IShoppingListItem> Items => Sections.SelectMany(s => s.Items).ToList().AsReadOnly();

        public void AddItem(IShoppingListItem item, SectionId sectionId)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (sectionId == null)
                throw new ArgumentNullException(nameof(sectionId));

            if (Items.Any(it => it.Id == item.Id))
                throw new DomainException(new ItemAlreadyOnShoppingListReason(item.Id, Id));

            if (!_sections.ContainsKey(sectionId))
                throw new DomainException(new SectionNotPartOfStoreReason(sectionId, StoreId));

            var section = _sections[sectionId];
            _sections[sectionId] = section.AddItem(item);
        }

        public void RemoveItem(ItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            IShoppingListSection section = _sections.Values.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                return;

            _sections[section.Id] = section.RemoveItem(itemId);
        }

        public void PutItemInBasket(ItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            IShoppingListSection section = _sections.Values.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

            _sections[section.Id] = section.PutItemInBasket(itemId);
        }

        public void RemoveFromBasket(ItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            IShoppingListSection section = _sections.Values.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

            _sections[section.Id] = section.RemoveItemFromBasket(itemId);
        }

        public void ChangeItemQuantity(ItemId itemId, float quantity)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));
            if (quantity <= 0f)
                throw new DomainException(new InvalidItemQuantityReason(quantity));

            IShoppingListSection section = _sections.Values.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

            _sections[section.Id] = section.ChangeItemQuantity(itemId, quantity);
        }

        public void AddSection(IShoppingListSection section)
        {
            if (section is null)
                throw new ArgumentNullException(nameof(section));

            if (_sections.ContainsKey(section.Id))
                throw new DomainException(new SectionAlreadyInShoppingListReason(Id, section.Id));

            _sections.Add(section.Id, section);
        }

        public IShoppingList Finish(DateTime completionDate)
        {
            if (CompletionDate != null)
                throw new DomainException(new ShoppingListAlreadyFinishedReason(Id));

            CompletionDate = completionDate;

            var notInBasketSections = new Dictionary<SectionId, IShoppingListSection>(_sections);
            foreach (SectionId key in _sections.Keys)
            {
                notInBasketSections[key] = notInBasketSections[key].RemoveItemsInBasket();
            }

            foreach (SectionId key in notInBasketSections.Keys)
            {
                _sections[key] = _sections[key].RemoveItemsNotInBasket();
            }

            return new ShoppingList(new ShoppingListId(0), StoreId, null, notInBasketSections.Values);
        }
    }
}