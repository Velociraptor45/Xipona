using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
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
                QuantityType = model.QuantityType.Id,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = model.QuantityInPacketType.Id,
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
                Comment = model.Comment,
                QuantityType = model.QuantityType.Id,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = model.QuantityInPacketType.Id,
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
                QuantityType = model.QuantityType.Id,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = model.QuantityInPacketType.Id,
                ItemCategoryId = model.ItemCategoryId,
                ManufacturerId = model.ManufacturerId,
                Availabilities = model.Availabilities.Select(av => av.ToItemAvailabilityContract())
            };
        }
    }
}