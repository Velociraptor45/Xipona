using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItem : IStoreItem
    {
        private IEnumerable<IStoreItemAvailability> availabilities;

        public StoreItem(StoreItemId id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            IItemCategory itemCategory, IManufacturer manufacturer,
            IEnumerable<IStoreItemAvailability> availabilities)
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

            // predecessor must be explicitly set via SetPredecessor(...) due to this AutoFixture bug:
            // https://github.com/AutoFixture/AutoFixture/issues/1108
            Predecessor = null;
        }

        public StoreItemId Id { get; }
        public string Name { get; private set; }
        public bool IsDeleted { get; private set; }
        public string Comment { get; private set; }
        public bool IsTemporary { get; private set; }
        public QuantityType QuantityType { get; private set; }
        public float QuantityInPacket { get; private set; }
        public QuantityTypeInPacket QuantityTypeInPacket { get; private set; }

        public IItemCategory ItemCategory { get; private set; }
        public IManufacturer Manufacturer { get; private set; }
        public IStoreItem Predecessor { get; private set; }

        public IReadOnlyCollection<IStoreItemAvailability> Availabilities => availabilities.ToList().AsReadOnly();

        public void Delete()
        {
            IsDeleted = true;
        }

        public bool IsAvailableInStore(StoreItemStoreId storeId)
        {
            return Availabilities.FirstOrDefault(av => av.StoreId == storeId) != null;
        }

        public void MakePermanent(PermanentItem permanentItem, IItemCategory itemCategory, IManufacturer manufacturer,
            IEnumerable<IStoreItemAvailability> availabilities)
        {
            Name = permanentItem.Name;
            Comment = permanentItem.Comment;
            QuantityType = permanentItem.QuantityType;
            QuantityInPacket = permanentItem.QuantityInPacket;
            QuantityTypeInPacket = permanentItem.QuantityTypeInPacket;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            this.availabilities = availabilities;
            IsTemporary = false;
        }

        public void Modify(ItemModify itemChange, IItemCategory itemCategory, IManufacturer manufacturer,
            IEnumerable<IStoreItemAvailability> availabilities)
        {
            Name = itemChange.Name;
            Comment = itemChange.Comment;
            QuantityType = itemChange.QuantityType;
            QuantityInPacket = itemChange.QuantityInPacket;
            QuantityTypeInPacket = itemChange.QuantityTypeInPacket;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            this.availabilities = availabilities;
        }

        public IStoreItemSection GetDefaultSectionForStore(StoreItemStoreId storeId)
        {
            var availability = availabilities.FirstOrDefault(av => av.StoreId == storeId);
            if (availability == null)
                throw new DomainException(new ItemAtStoreNotAvailableReason(Id, storeId));

            return availability.DefaultSection;
        }

        public void SetPredecessor(IStoreItem predecessor)
        {
            Predecessor = predecessor;
        }
    }
}