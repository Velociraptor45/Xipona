using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class ShoppingListRoot
    {
        private readonly Dictionary<int, ShoppingListSection> sections;

        public ShoppingListRoot(int id, DateTime? completionDate, Store store, IEnumerable<ShoppingListSection> sections)
        {
            Id = id;
            CompletionDate = completionDate;
            Store = store;
            this.sections = sections.ToDictionary(s => s.Id);
        }

        public int Id { get; }
        public DateTime? CompletionDate { get; }
        public Store Store { get; }
        public IReadOnlyCollection<ShoppingListSection> Sections => sections.Values.ToList().AsReadOnly();
        public IReadOnlyCollection<ShoppingListItem> Items => Sections.SelectMany(s => s.Items).ToList().AsReadOnly();
        public bool AnyItemInBasket => Items.Any(item => item.IsInBasket);

        public int DefaultSectionId => Sections.Single(s => s.IsDefaultSection).Id;

        public ShoppingListItem GetItemById(int id)
        {
            return Items.FirstOrDefault(item => item.Id.ActualId == id);
        }

        public void Remove(ItemId itemId)
        {
            var section = sections.Values.FirstOrDefault(s => s.Items.Any(i => i.Id == itemId));

            section.RemoveItem(itemId);
        }

        public void AddItem(ShoppingListItem item, int sectionId)
        {
            if (!sections.TryGetValue(sectionId, out var section))
                return;

            section.AddItem(item);
        }
    }
}