using ProjectHermes.ShoppingList.Frontend.Models.Index.Services;
using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class ShoppingListSection
    {
        private readonly Dictionary<(ItemId, Guid?), ShoppingListItem> _items;

        public ShoppingListSection(Guid id, string name, int sortingIndex, bool isDefaultSection,
            IEnumerable<ShoppingListItem> items)
        {
            _items = items.ToDictionary(i => (i.Id, i.TypeId));
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
            IsExpanded = true;
        }

        public Guid Id { get; }
        public string Name { get; }
        public int SortingIndex { get; }
        public bool IsDefaultSection { get; }
        public bool IsExpanded { get; private set; }
        public bool AllItemsInBasket => Items.All(i => i.IsInBasket);
        public bool SomeItemsInBasket => !AllItemsInBasket && Items.Any(i => i.IsInBasket);
        public IReadOnlyCollection<ShoppingListItem> Items => _items.Values.ToList().AsReadOnly();

        public void RemoveItem(ItemId itemId, Guid? itemTypeId)
        {
            _items.Remove((itemId, itemTypeId));
        }

        public void AddItem(ShoppingListItem item)
        {
            if (_items.ContainsKey((item.Id, item.TypeId)))
                return;

            _items.Add((item.Id, item.TypeId), item);
        }

        public void Expand()
        {
            IsExpanded = true;
        }

        public void Close()
        {
            IsExpanded = false;
        }

        public float GetTotalPrice(IItemPriceCalculationService priceCalculationService)
        {
            return Items.Where(i => !i.IsTemporary).Sum(i => i.GetTotalPrice(priceCalculationService));
        }

        public float GetInBasketPrice(IItemPriceCalculationService priceCalculationService)
        {
            return Items.Where(i => i.IsInBasket && !i.IsTemporary).Sum(i => i.GetTotalPrice(priceCalculationService));
        }
    }
}