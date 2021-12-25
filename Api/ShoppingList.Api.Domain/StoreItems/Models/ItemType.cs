using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class ItemType : IItemType
    {
        public ItemType(ItemTypeId id, string name, IEnumerable<IStoreItemAvailability> availabilities)
        {
            Id = id;
            Name = name;
            Availabilities = availabilities.ToList();

            // predecessor must be explicitly set via SetPredecessor(...) due to this AutoFixture bug:
            // https://github.com/AutoFixture/AutoFixture/issues/1108
            Predecessor = null;
        }

        public ItemTypeId Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<IStoreItemAvailability> Availabilities { get; }
        public IItemType? Predecessor { get; private set; }

        public void SetPredecessor(IItemType predecessor)
        {
            Predecessor = predecessor;
        }

        public SectionId GetDefaultSectionIdForStore(StoreId storeId)
        {
            if (storeId is null)
                throw new ArgumentNullException(nameof(storeId));

            var availability = Availabilities.FirstOrDefault(av => av.StoreId == storeId);
            if (availability == null)
                throw new DomainException(new ItemTypeAtStoreNotAvailableReason(Id, storeId));

            return availability.DefaultSectionId;
        }
    }
}