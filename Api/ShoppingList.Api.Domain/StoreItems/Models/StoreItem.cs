using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.Models
{
    public class StoreItem
    {
        private IEnumerable<StoreItemAvailability> availabilities;

        public StoreItem(StoreItemId id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategory itemCategory, Manufacturer manufacturer,
            IEnumerable<StoreItemAvailability> availabilities, StoreItem predecessor)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            this.availabilities = availabilities ?? throw new ArgumentNullException(nameof(availabilities));
            Predecessor = predecessor;
        }

        public StoreItemId Id { get; }
        public string Name { get; private set; }
        public bool IsDeleted { get; private set; }
        public string Comment { get; private set; }
        public bool IsTemporary { get; private set; }
        public QuantityType QuantityType { get; private set; }
        public float QuantityInPacket { get; private set; }
        public QuantityTypeInPacket QuantityTypeInPacket { get; private set; }

        public ItemCategory ItemCategory { get; private set; }
        public Manufacturer Manufacturer { get; private set; }
        public StoreItem Predecessor { get; private set; }

        public IReadOnlyCollection<StoreItemAvailability> Availabilities => availabilities.ToList().AsReadOnly();

        public void Delete()
        {
            IsDeleted = true;
        }

        public bool IsAvailableInStore(StoreId storeId)
        {
            return Availabilities.FirstOrDefault(av => av.StoreId == storeId) != null;
        }

        public void MakePermanent(PermanentItem permanentItem, ItemCategory itemCategory, Manufacturer manufacturer)
        {
            Name = permanentItem.Name;
            Comment = permanentItem.Comment;
            QuantityType = permanentItem.QuantityType;
            QuantityInPacket = permanentItem.QuantityInPacket;
            QuantityTypeInPacket = permanentItem.QuantityTypeInPacket;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            availabilities = permanentItem.Availabilities;
            IsTemporary = false;
        }

        public void Modify(ItemModify itemChange, ItemCategory itemCategory, Manufacturer manufacturer)
        {
            Name = itemChange.Name;
            Comment = itemChange.Comment;
            QuantityType = itemChange.QuantityType;
            QuantityInPacket = itemChange.QuantityInPacket;
            QuantityTypeInPacket = itemChange.QuantityTypeInPacket;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            availabilities = itemChange.Availabilities;
        }

        public void SetPredecessor(StoreItem predecessor)
        {
            Predecessor = predecessor;
        }
    }
}