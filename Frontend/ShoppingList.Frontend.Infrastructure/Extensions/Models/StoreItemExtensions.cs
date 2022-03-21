using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Models
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
                QuantityType = model.QuantityType.Id,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = model.QuantityInPacketType?.Id,
                ItemCategoryId = model.ItemCategoryId.Value,
                ManufacturerId = model.ManufacturerId,
                Availabilities = model.Availabilities.Select(av => av.ToItemAvailabilityContract())
            };
        }

        public static UpdateItemWithTypesContract ToUpdateItemWithTypesContract(this StoreItem model)
        {
            return new UpdateItemWithTypesContract()
            {
                OldId = model.Id,
                Name = model.Name,
                Comment = model.Comment,
                QuantityType = model.QuantityType.Id,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = model.QuantityInPacketType?.Id,
                ItemCategoryId = model.ItemCategoryId.Value,
                ManufacturerId = model.ManufacturerId,
                ItemTypes = model.ItemTypes.Select(t => t.ToUpdateItemTypeContract())
            };
        }

        public static ModifyItemContract ToModifyItemContract(this StoreItem model)
        {
            return new ModifyItemContract()
            {
                Id = model.Id,
                Name = model.Name,
                Comment = model.Comment,
                QuantityType = model.QuantityType.Id,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = model.QuantityInPacketType?.Id,
                ItemCategoryId = model.ItemCategoryId.Value,
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
                QuantityType = model.QuantityType.Id,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = model.QuantityInPacketType?.Id,
                ItemCategoryId = model.ItemCategoryId.Value,
                ManufacturerId = model.ManufacturerId,
                Availabilities = model.Availabilities.Select(av => av.ToItemAvailabilityContract())
            };
        }

        public static CreateItemWithTypesContract ToCreateItemWithTypesContract(this StoreItem model)
        {
            return new CreateItemWithTypesContract()
            {
                Name = model.Name,
                Comment = model.Comment,
                QuantityType = model.QuantityType.Id,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = model.QuantityInPacketType?.Id,
                ItemCategoryId = model.ItemCategoryId.Value,
                ManufacturerId = model.ManufacturerId,
                ItemTypes = model.ItemTypes.Select(t => t.ToCreateItemTypeContract())
            };
        }
    }
}