using ProjectHermes.ShoppingList.Frontend.Models.Index.Services;
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

        public ShoppingListRoot(Guid id, DateTimeOffset? completionDate, Store store, IEnumerable<ShoppingListSection> sections)
        {
            Id = id;
            CompletionDate = completionDate;
            Store = store;
            this.sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer());
        }

        public Guid Id { get; }
        public DateTimeOffset? CompletionDate { get; }
        public Store Store { get; }
        public IReadOnlyCollection<ShoppingListSection> Sections => sections.ToList().AsReadOnly();
        public IReadOnlyCollection<ShoppingListItem> Items => Sections.SelectMany(s => s.Items).ToList().AsReadOnly();
        public bool AnyItemInBasket => Items.Any(item => item.IsInBasket);

        public int ItemInBasketCount => Items.Count(i => i.IsInBasket);
        public int ItemNotInBasketCount => Items.Count(i => !i.IsInBasket);

        public ShoppingListItem GetItemById(Guid id)
        {
            return Items.FirstOrDefault(item => item.Id.ActualId == id);
        }

        public void Remove(ItemId itemId, Guid? itemTypeId)
        {
            var section = sections.FirstOrDefault(s => s.Items.Any(i => i.Id == itemId));

            section?.RemoveItem(itemId, itemTypeId);
        }

        public void AddItem(ShoppingListItem item, Guid sectionId)
        {
            var section = sections.FirstOrDefault(s => s.Id == sectionId);

            section?.AddItem(item);
        }

        public float GetTotalPrice(IItemPriceCalculationService priceCalculationService)
        {
            return Sections.Sum(s => s.GetTotalPrice(priceCalculationService));
        }

        public float GetInBasketPrice(IItemPriceCalculationService priceCalculationService)
        {
            return Sections.Sum(s => s.GetInBasketPrice(priceCalculationService));
        }

        public string GetFormattedInBasketPrice(IItemPriceCalculationService priceCalculationService)
        {
            var price = GetInBasketPrice(priceCalculationService);
            return $"{price:0.00}";
        }

        public IReadOnlyCollection<ShoppingListSection> GetNonEmptySections(bool excludeWithAllItemsInBasket)
        {
            return sections.Where(s => s.Items.Any() && (!s.AllItemsInBasket || !excludeWithAllItemsInBasket)).ToList();
        }
    }
}