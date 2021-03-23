using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItem : IStoreItem
    {
        private List<IStoreItemAvailability> availabilities;

        public StoreItem(ItemId id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategoryId itemCategoryId, ManufacturerId manufacturerId,
            IEnumerable<IStoreItemAvailability> availabilities, TemporaryItemId temporaryId)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategoryId = itemCategoryId;
            ManufacturerId = manufacturerId;
            TemporaryId = temporaryId;
            this.availabilities = availabilities.ToList() ?? throw new ArgumentNullException(nameof(availabilities));

            // predecessor must be explicitly set via SetPredecessor(...) due to this AutoFixture bug:
            // https://github.com/AutoFixture/AutoFixture/issues/1108
            Predecessor = null;
        }

        public ItemId Id { get; }
        public string Name { get; private set; }
        public bool IsDeleted { get; private set; }
        public string Comment { get; private set; }
        public bool IsTemporary { get; private set; }
        public QuantityType QuantityType { get; private set; }
        public float QuantityInPacket { get; private set; }
        public QuantityTypeInPacket QuantityTypeInPacket { get; private set; }

        public ItemCategoryId ItemCategoryId { get; private set; }
        public ManufacturerId ManufacturerId { get; private set; }
        public TemporaryItemId TemporaryId { get; }
        public IStoreItem Predecessor { get; private set; } // todo: change this to an IItemPredecessor model to satisfy DDD

        public IReadOnlyCollection<IStoreItemAvailability> Availabilities => availabilities.AsReadOnly();

        public void Delete()
        {
            IsDeleted = true;
        }

        public bool IsAvailableInStore(StoreId storeId)
        {
            return Availabilities.Any(av => av.StoreId == storeId);
        }

        public void MakePermanent(PermanentItem permanentItem, IEnumerable<IStoreItemAvailability> availabilities)
        {
            Name = permanentItem.Name;
            Comment = permanentItem.Comment;
            QuantityType = permanentItem.QuantityType;
            QuantityInPacket = permanentItem.QuantityInPacket;
            QuantityTypeInPacket = permanentItem.QuantityTypeInPacket;
            ItemCategoryId = permanentItem.ItemCategoryId;
            ManufacturerId = permanentItem.ManufacturerId;
            this.availabilities = availabilities.ToList();
            IsTemporary = false;
        }

        public void Modify(ItemModify itemChange, IEnumerable<IStoreItemAvailability> availabilities)
        {
            Name = itemChange.Name;
            Comment = itemChange.Comment;
            QuantityType = itemChange.QuantityType;
            QuantityInPacket = itemChange.QuantityInPacket;
            QuantityTypeInPacket = itemChange.QuantityTypeInPacket;
            ItemCategoryId = itemChange.ItemCategoryId;
            ManufacturerId = itemChange.ManufacturerId;
            this.availabilities = availabilities.ToList();
        }

        public SectionId GetDefaultSectionForStore(StoreId storeId)
        {
            var availability = availabilities.FirstOrDefault(av => av.StoreId == storeId);
            if (availability == null)
                throw new DomainException(new ItemAtStoreNotAvailableReason(Id, storeId));

            return availability.DefaultSectionId;
        }

        public void SetPredecessor(IStoreItem predecessor)
        {
            Predecessor = predecessor;
        }
    }
}