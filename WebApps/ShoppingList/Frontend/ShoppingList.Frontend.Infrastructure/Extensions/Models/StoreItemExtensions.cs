using ShoppingList.Api.Contracts.Commands.ChangeItem;
using ShoppingList.Api.Contracts.Commands.CreateItem;
using ShoppingList.Api.Contracts.Commands.UpdateItem;
using ShoppingList.Frontend.Models.Items;
using System.Linq;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Models
{
    public static class StoreItemExtensions
    {
        public static UpdateItemContract ToUpdateItemContract(this StoreItem model)
        {
            return new UpdateItemContract()
            {
                OldId = model.Id,
                Name = model.Name,
                Comment = model.Comment,
                QuantityType = model.QuantityType,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = model.QuantityInPacketType,
                ItemCategoryId = model.ItemCategoryId,
                ManufacturerId = model.ManufacturerId,
                Availabilities = model.Availabilities.Select(av => av.ToItemAvailabilityContract())
            };
        }

        public static ChangeItemContract ToChangeItemContract(this StoreItem model)
        {
            return new ChangeItemContract()
            {
                Id = model.Id,
                Name = model.Name,
                IsDeleted = model.IsDeleted,
                Comment = model.Comment,
                IsTemporary = model.IsTemporary,
                QuantityType = model.QuantityType,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = model.QuantityInPacketType,
                ItemCategoryId = model.ItemCategoryId,
                ManufacturerId = model.ManufacturerId,
                Availabilities = model.Availabilities.Select(av => av.ToItemAvailabilityContract())
            };
        }

        public static CreateItemContract ToCreateItemContract(this StoreItem model)
        {
            return new CreateItemContract()
            {
                Name = model.Name,
                Comment = model.Comment,
                IsTemporary = model.IsTemporary,
                QuantityType = model.QuantityType,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = model.QuantityInPacketType,
                ItemCategoryId = model.ItemCategoryId,
                ManufacturerId = model.ManufacturerId,
                Availabilities = model.Availabilities.Select(av => av.ToItemAvailabilityContract())
            };
        }
    }
}