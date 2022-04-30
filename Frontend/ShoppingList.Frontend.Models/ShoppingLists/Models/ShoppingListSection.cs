using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models
{
    public class ShoppingListSection
    {
        private readonly Dictionary<(ShoppingListItemId, Guid?), ShoppingListItem> _items;

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

        public void RemoveItem(ShoppingListItemId itemId, Guid? itemTypeId)
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
            return Items.Sum(i => i.GetTotalPrice(priceCalculationService));
        }

        public float GetInBasketPrice(IItemPriceCalculationService priceCalculationService)
        {
            return Items.Where(i => i.IsInBasket).Sum(i => i.GetTotalPrice(priceCalculationService));
        }

        public static ShoppingListSection From(Section section)
        {
            return new ShoppingListSection(section.Id.BackendId, section.Name, section.SortingIndex,
                section.IsDefaultSection, Enumerable.Empty<ShoppingListItem>());
        }
    }
}