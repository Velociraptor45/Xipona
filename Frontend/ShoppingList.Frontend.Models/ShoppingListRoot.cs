using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Comparer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class ShoppingListRoot
    {
        private readonly SortedSet<ShoppingListSection> sections;

        public ShoppingListRoot(int id, DateTime? completionDate, Store store, IEnumerable<ShoppingListSection> sections)
        {
            Id = id;
            CompletionDate = completionDate;
            Store = store;
            this.sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer());
        }

        public int Id { get; }
        public DateTime? CompletionDate { get; }
        public Store Store { get; }
        public IReadOnlyCollection<ShoppingListSection> Sections => sections.ToList().AsReadOnly();
        public IReadOnlyCollection<ShoppingListItem> Items => Sections.SelectMany(s => s.Items).ToList().AsReadOnly();
        public bool AnyItemInBasket => Items.Any(item => item.IsInBasket);

        public int DefaultSectionId => Sections.Single(s => s.IsDefaultSection).Id;

        public ShoppingListItem GetItemById(int id)
        {
            return Items.FirstOrDefault(item => item.Id.ActualId == id);
        }

        public void Remove(ItemId itemId)
        {
            var section = sections.FirstOrDefault(s => s.Items.Any(i => i.Id == itemId));

            section?.RemoveItem(itemId);
        }

        public void AddItem(ShoppingListItem item, int sectionId)
        {
            var section = sections.FirstOrDefault(s => s.Id == sectionId);

            section?.AddItem(item);
        }
    }
}