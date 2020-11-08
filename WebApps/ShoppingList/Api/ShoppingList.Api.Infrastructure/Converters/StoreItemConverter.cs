using ShoppingList.Api.Infrastructure.Entities;
using System.Linq;
using Models = ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Infrastructure.Converters
{
    public static class StoreItemConverter
    {
        public static Models.StoreItem ToStoreItemDomain(this Item entity)
        {
            return new Models.StoreItem(new Models.StoreItemId(entity.Id),
                entity.Name,
                entity.Deleted,
                entity.Comment,
                entity.IsTemporary,
                (Models.QuantityType)entity.QuantityType,
                entity.QuantityInPacket,
                (Models.QuantityTypeInPacket)entity.QuantityTypeInPacket,
                entity.ItemCategory.ToDomain(),
                entity.Manufacturer.ToDomain(),
                entity.AvailableAt.Select(map => new Models.StoreItemAvailability(
                    new Models.StoreId(map.StoreId), map.Price)));
        }

        public static Item ToEntity(this Models.StoreItem model)
        {
            return new Item()
            {
                Id = model.Id.Value,
                Name = model.Name,
                Deleted = model.IsDeleted,
                Comment = model.Comment,
                IsTemporary = model.IsTemporary,
                QuantityType = (int)model.QuantityType,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = (int)model.QuantityTypeInPacket,
                ItemCategoryId = model.ItemCategory.Id.Value,
                ManufacturerId = model.Manufacturer.Id.Value
            };
        }
    }
}