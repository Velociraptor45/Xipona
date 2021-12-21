using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModification;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItem : IStoreItem
    {
        private List<IStoreItemAvailability> _availabilities;
        private List<IItemType> _itemTypes;

        public StoreItem(ItemId id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId,
            IEnumerable<IStoreItemAvailability> availabilities, TemporaryItemId? temporaryId)
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
            _itemTypes = new List<IItemType>();
            _availabilities = availabilities.ToList() ?? throw new ArgumentNullException(nameof(availabilities));

            // predecessor must be explicitly set via SetPredecessor(...) due to this AutoFixture bug:
            // https://github.com/AutoFixture/AutoFixture/issues/1108
            Predecessor = null;
        }

        public StoreItem(ItemId id, string name, bool isDeleted, string comment,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId,
            IEnumerable<IItemType> itemTypes)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = false;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategoryId = itemCategoryId;
            ManufacturerId = manufacturerId;
            TemporaryId = null;
            _itemTypes = itemTypes?.ToList() ?? throw new ArgumentNullException(nameof(itemTypes));
            _availabilities = new List<IStoreItemAvailability>();

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

        public ItemCategoryId? ItemCategoryId { get; private set; }
        public ManufacturerId? ManufacturerId { get; private set; }
        public TemporaryItemId? TemporaryId { get; }
        public IStoreItem? Predecessor { get; private set; } // todo: change this to an IItemPredecessor model to satisfy DDD
        public IReadOnlyCollection<IItemType> ItemTypes => _itemTypes.AsReadOnly();

        public IReadOnlyCollection<IStoreItemAvailability> Availabilities => _availabilities.AsReadOnly();

        public bool HasItemTypes => _itemTypes.Any();

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
            _availabilities = availabilities.ToList();
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
            _availabilities = availabilities.ToList();
        }

        public async Task ModifyAsync(ItemWithTypesModification modification, IValidator validator)
        {
            if (modification is null)
                throw new ArgumentNullException(nameof(modification));

            if (_availabilities.Any())
                throw new DomainException(new CannotModifyItemAsItemWithTypesReason(Id));

            foreach (var type in modification.ItemTypes)
            {
                await validator.ValidateAsync(type.Availabilities);
            }

            Name = modification.Name;
            Comment = modification.Comment;
            QuantityType = modification.QuantityType;
            QuantityInPacket = modification.QuantityInPacket;
            QuantityTypeInPacket = modification.QuantityTypeInPacket;
            ItemCategoryId = modification.ItemCategoryId;
            ManufacturerId = modification.ManufacturerId;
            _itemTypes = modification.ItemTypes.ToList();
        }

        public SectionId GetDefaultSectionIdForStore(StoreId storeId)
        {
            var availability = _availabilities.FirstOrDefault(av => av.StoreId == storeId);
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