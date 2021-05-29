using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public interface IStoreItem
    {
        ItemId Id { get; }
        string Name { get; }
        bool IsDeleted { get; }
        string Comment { get; }
        bool IsTemporary { get; }
        QuantityType QuantityType { get; }
        float QuantityInPacket { get; }
        QuantityTypeInPacket QuantityTypeInPacket { get; }

        ItemCategoryId ItemCategoryId { get; }
        ManufacturerId ManufacturerId { get; }
        IStoreItem Predecessor { get; }

        IReadOnlyCollection<IStoreItemAvailability> Availabilities { get; }
        TemporaryItemId TemporaryId { get; }

        void Delete();

        SectionId GetDefaultSectionIdForStore(StoreId storeId);

        bool IsAvailableInStore(StoreId storeId);

        void MakePermanent(PermanentItem permanentItem, IEnumerable<IStoreItemAvailability> availabilities);

        void Modify(ItemModify itemChange, IEnumerable<IStoreItemAvailability> availabilities);

        void SetPredecessor(IStoreItem predecessor);
    }
}