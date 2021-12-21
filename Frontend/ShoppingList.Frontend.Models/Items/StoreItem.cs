using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items
{
    public class StoreItem
    {
        public StoreItem(int id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityInPacketType,
            int? itemCategoryId, int? manufacturerId, IEnumerable<StoreItemAvailability> availabilities,
            IEnumerable<ItemType> itemTypes)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityInPacketType = quantityInPacketType;
            ItemCategoryId = itemCategoryId;
            ManufacturerId = manufacturerId;
            ItemTypes = itemTypes.ToList();
            Availabilities = availabilities.ToList();
            SetItemMode();
        }

        public int Id { get; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string Comment { get; set; }
        public bool IsTemporary { get; set; }
        public QuantityType QuantityType { get; set; }
        public float QuantityInPacket { get; set; }
        public QuantityTypeInPacket QuantityInPacketType { get; set; }
        public int? ItemCategoryId { get; set; }
        public int? ManufacturerId { get; set; }
        public List<ItemType> ItemTypes { get; set; }
        public List<StoreItemAvailability> Availabilities { get; set; }
        public ItemMode ItemMode { get; private set; }

        public bool IsItemWithTypes => ItemMode == ItemMode.WithTypes;

        public IEnumerable<StoreItemStore> GetNotRegisteredStores(IEnumerable<Store> stores)
        {
            var registeredStoreIds = Availabilities.Select(av => av.Store.Id).OrderBy(id => id);
            var allStoreIds = stores.Select(s => s.Id).OrderBy(id => id);

            if (allStoreIds.SequenceEqual(registeredStoreIds))
                return Enumerable.Empty<StoreItemStore>();

            var availableStoreIds = allStoreIds.Except(registeredStoreIds).ToList();
            return stores.Where(s => availableStoreIds.Contains(s.Id)).Select(s => s.AsStoreItemStore());
        }

        private void SetItemMode()
        {
            if (Id == 0)
            {
                ItemMode = ItemMode.NotDefined;
                return;
            }

            ItemMode = ItemTypes.Count > 0 ? ItemMode.WithTypes : ItemMode.WithoutTypes;
        }
    }
}