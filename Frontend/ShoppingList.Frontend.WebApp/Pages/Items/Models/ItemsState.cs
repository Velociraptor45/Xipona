﻿using ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreModels = ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Models
{
    public class ItemsState
    {
        private List<StoreModels.Store> _stores = new();
        private List<ItemCategory> _itemCategories;
        private List<Manufacturer> _manufacturers;
        private List<SearchItemResult> _items = new();
        private List<QuantityType> _quantityTypes;
        private List<QuantityTypeInPacket> _quantityTypesInPacket;

        public IReadOnlyCollection<StoreModels.Store> Stores => _stores.AsReadOnly();
        public IReadOnlyCollection<ItemCategory> ItemCategories => _itemCategories.AsReadOnly();
        public IReadOnlyCollection<Manufacturer> Manufacturers => _manufacturers.AsReadOnly();
        public IReadOnlyCollection<SearchItemResult> Items => _items.AsReadOnly();
        public IReadOnlyCollection<QuantityType> QuantityTypes => _quantityTypes.AsReadOnly();
        public IReadOnlyCollection<QuantityTypeInPacket> QuantityTypesInPacket => _quantityTypesInPacket.AsReadOnly();

        public Action StateChanged { get; set; }
        public Item EditedItem { get; private set; }

        public void Initialize(IEnumerable<StoreModels.Store> stores, IEnumerable<ItemCategory> itemCategories,
            IEnumerable<Manufacturer> manufacturers, IEnumerable<QuantityType> quantityTypes,
            IEnumerable<QuantityTypeInPacket> quantityTypesInPacket)
        {
            _stores = stores.ToList();
            _itemCategories = itemCategories.ToList();
            _manufacturers = manufacturers.ToList();
            _quantityTypes = quantityTypes.ToList();
            _quantityTypesInPacket = quantityTypesInPacket.ToList();
        }

        public void ClearItems()
        {
            UpdateItems(Enumerable.Empty<SearchItemResult>());
        }

        public void UpdateItems(IEnumerable<SearchItemResult> items)
        {
            _items = items.ToList();
            StateChanged?.Invoke();
        }

        public StoreModels.Store GetStore(Guid id)
        {
            return Stores.FirstOrDefault(s => s.Id == id);
        }

        public void SetNewEditedItem()
        {
            // todo: ugly
            var item =
                new Item(Guid.Empty, "", false, "", false,
                    new QuantityType(0, "", 0, "", "", 0), 0,
                    new QuantityTypeInPacket(0, "", ""), null, null,
                    new List<ItemAvailability>(),
                    new List<ItemType>());

            SetEditedItem(item);
        }

        public void SetEditedItem(Item item)
        {
            EditedItem = item;
            StateChanged?.Invoke();
        }
    }
}