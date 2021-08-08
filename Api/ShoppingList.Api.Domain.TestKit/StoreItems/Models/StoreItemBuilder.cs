using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Core.TestKit;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models
{
    public class StoreItemBuilder : TestBuilderBase<StoreItem>
    {
        public StoreItemBuilder WithId(ItemId id)
        {
            FillContructorWith("id", id);
            return this;
        }

        public StoreItemBuilder WithIsDeleted(bool isDeleted)
        {
            FillContructorWith("isDeleted", isDeleted);
            return this;
        }

        public StoreItemBuilder WithIsTemporary(bool isTemporary)
        {
            FillContructorWith("isTemporary", isTemporary);
            return this;
        }

        public StoreItemBuilder WithQuantityType(QuantityType quantityType)
        {
            FillContructorWith("quantityType", quantityType);
            return this;
        }

        public StoreItemBuilder WithQuantityInPacket(float quantityInPacket)
        {
            FillContructorWith("quantityInPacket", quantityInPacket);
            return this;
        }

        public StoreItemBuilder WithQuantityTypeInPacket(QuantityTypeInPacket quantityTypeInPacket)
        {
            FillContructorWith("quantityTypeInPacket", quantityTypeInPacket);
            return this;
        }

        public StoreItemBuilder WithItemCategoryId(ItemCategoryId itemCategoryId)
        {
            FillContructorWith("itemCategoryId", itemCategoryId);
            return this;
        }

        public StoreItemBuilder WithoutItemCategoryId()
        {
            return WithItemCategoryId(null);
        }

        public StoreItemBuilder WithManufacturerId(ManufacturerId manufacturerId)
        {
            FillContructorWith("manufacturerId", manufacturerId);
            return this;
        }

        public StoreItemBuilder WithoutManufacturerId()
        {
            return WithManufacturerId(null);
        }

        public StoreItemBuilder WithAvailabilities(IEnumerable<IStoreItemAvailability> availabilities)
        {
            FillContructorWith("availabilities", availabilities);
            return this;
        }

        public StoreItemBuilder WithTemporaryId(TemporaryItemId temporaryId)
        {
            FillContructorWith("temporaryId", temporaryId);
            return this;
        }
    }
}