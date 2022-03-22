using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class UpdateItemContractConverter :
        IToContractConverter<StoreItem, UpdateItemContract>
    {
        private readonly IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter;

        public UpdateItemContractConverter(
            IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            this.availabilityConverter = availabilityConverter;
        }

        public UpdateItemContract ToContract(StoreItem source)
        {
            return new UpdateItemContract
            {
                OldId = source.Id,
                Name = source.Name,
                Comment = source.Comment,
                QuantityType = source.QuantityType.Id,
                QuantityInPacket = source.QuantityInPacket,
                QuantityTypeInPacket = source.QuantityInPacketType?.Id,
                ItemCategoryId = source.ItemCategoryId.Value,
                ManufacturerId = source.ManufacturerId,
                Availabilities = source.Availabilities.Select(availabilityConverter.ToContract)
            };
        }
    }
}