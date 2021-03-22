using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingList : IShoppingList
    {
        private readonly Dictionary<ShoppingListSectionId, IShoppingListSection> shoppingListSections;

        public ShoppingList(ShoppingListId id, ShoppingListStoreId storeId, DateTime? completionDate,
            IEnumerable<IShoppingListSection> sections)
        {
            Id = id;
            StoreId = storeId;
            CompletionDate = completionDate;
            this.shoppingListSections = sections.ToDictionary(s => s.Id);
        }

        public ShoppingListId Id { get; }
        public ShoppingListStoreId StoreId { get; }
        public DateTime? CompletionDate { get; }

        public IReadOnlyCollection<IShoppingListSection> Sections => shoppingListSections.Values.ToList().AsReadOnly();
        public IReadOnlyCollection<IShoppingListItem> Items => Sections.SelectMany(s => s.Items).ToList().AsReadOnly();

        public IShoppingList AddItem(IShoppingListItem item, ShoppingListSectionId sectionId)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (sectionId == null)
                throw new ArgumentNullException(nameof(sectionId));

            var existingItem = Items.FirstOrDefault(it => it.Id == item.Id);
            if (existingItem != null)
                throw new DomainException(new ItemAlreadyOnShoppingListReason(item.Id, Id));

            if (!shoppingListSections.ContainsKey(sectionId))
                throw new DomainException(new SectionNotPartOfStoreReason(sectionId, StoreId));

            var sections = new Dictionary<ShoppingListSectionId, IShoppingListSection>(shoppingListSections);
            var section = sections[sectionId];
            sections[sectionId] = section.AddItem(item);

            return new ShoppingList(Id, StoreId, CompletionDate, sections.Values);
        }

        public IShoppingList RemoveItem(ShoppingListItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            IShoppingListSection section = shoppingListSections.Values.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                return this;

            var sections = new Dictionary<ShoppingListSectionId, IShoppingListSection>(shoppingListSections)
            {
                [section.Id] = section.RemoveItem(itemId)
            };

            return new ShoppingList(Id, StoreId, CompletionDate, sections.Values);
        }

        public IShoppingList PutItemInBasket(ShoppingListItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            IShoppingListSection section = shoppingListSections.Values.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

            var sections = new Dictionary<ShoppingListSectionId, IShoppingListSection>(shoppingListSections)
            {
                [section.Id] = section.PutItemInBasket(itemId)
            };

            return new ShoppingList(Id, StoreId, CompletionDate, sections.Values);
        }

        public IShoppingList RemoveFromBasket(ShoppingListItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            IShoppingListSection section = shoppingListSections.Values.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

            var sections = new Dictionary<ShoppingListSectionId, IShoppingListSection>(shoppingListSections)
            {
                [section.Id] = section.RemoveItemFromBasket(itemId)
            };

            return new ShoppingList(Id, StoreId, CompletionDate, sections.Values);
        }

        public IShoppingList ChangeItemQuantity(ShoppingListItemId itemId, float quantity)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));
            if (quantity <= 0f)
                throw new DomainException(new InvalidItemQuantityReason(quantity));

            IShoppingListSection section = shoppingListSections.Values.FirstOrDefault(s => s.ContainsItem(itemId));
            if (section == null)
                throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

            var sections = new Dictionary<ShoppingListSectionId, IShoppingListSection>(shoppingListSections)
            {
                [section.Id] = section.ChangeItemQuantity(itemId, quantity)
            };

            return new ShoppingList(Id, StoreId, CompletionDate, sections.Values);
        }

        public IShoppingList SetCompletionDate(DateTime completionDate)
        {
            return new ShoppingList(Id, StoreId, completionDate, shoppingListSections.Values);
        }

        public IShoppingList RemoveItemsInBasket()
        {
            var sections = new Dictionary<ShoppingListSectionId, IShoppingListSection>(shoppingListSections);
            foreach (var key in sections.Keys)
            {
                sections[key] = sections[key].RemoveItemsInBasket();
            }

            return new ShoppingList(Id, StoreId, CompletionDate, sections.Values);
        }

        public IShoppingList RemoveItemsNotInBasket()
        {
            var sections = new Dictionary<ShoppingListSectionId, IShoppingListSection>(shoppingListSections);
            foreach (var key in sections.Keys)
            {
                sections[key] = sections[key].RemoveItemsNotInBasket();
            }

            return new ShoppingList(Id, StoreId, CompletionDate, sections.Values);
        }
    }
}