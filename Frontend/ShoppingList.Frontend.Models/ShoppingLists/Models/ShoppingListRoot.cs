using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Comparer;
using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models
{
    public class ShoppingListRoot
    {
        private readonly SortedSet<ShoppingListSection> _sections;

        public ShoppingListRoot(Guid id, DateTimeOffset? completionDate, ShoppingListStore store,
            IEnumerable<ShoppingListSection> sections)
        {
            Id = id;
            CompletionDate = completionDate;
            Store = store;
            _sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer());
        }

        public Guid Id { get; }
        public DateTimeOffset? CompletionDate { get; }
        public ShoppingListStore Store { get; }
        public IReadOnlyCollection<ShoppingListSection> Sections => _sections.ToList().AsReadOnly();
        public IReadOnlyCollection<ShoppingListItem> Items => Sections.SelectMany(s => s.Items).ToList().AsReadOnly();
        public bool AnyItemInBasket => Items.Any(item => item.IsInBasket);

        public int ItemInBasketCount => Items.Count(i => i.IsInBasket);
        public int ItemNotInBasketCount => Items.Count(i => !i.IsInBasket);

        public void Remove(ShoppingListItemId itemId, Guid? itemTypeId)
        {
            var section = _sections.FirstOrDefault(s => s.Items.Any(i => i.Id == itemId));

            section?.RemoveItem(itemId, itemTypeId);
        }

        public void AddItem(ShoppingListItem item, Section storeSection)
        {
            var section = _sections.FirstOrDefault(s => s.Id == storeSection.Id.BackendId);

            if (section is null)
            {
                var newSection = ShoppingListSection.From(storeSection);
                _sections.Add(newSection);
                section = newSection;
            }

            section.AddItem(item);
        }

        public IReadOnlyCollection<ShoppingListSection> GetNonEmptySections(bool excludeWithAllItemsHidden)
        {
            return _sections.Where(s => s.Items.Any() && (!s.AllItemsHidden || !excludeWithAllItemsHidden)).ToList();
        }
    }
}