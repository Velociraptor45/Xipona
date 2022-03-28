using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class UpdateItemWithTypesContractConverter :
        IToContractConverter<StoreItem, UpdateItemWithTypesContract>
    {
        private readonly IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter;

        public UpdateItemWithTypesContractConverter(
            IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            this.availabilityConverter = availabilityConverter;
        }

        public UpdateItemWithTypesContract ToContract(StoreItem source)
        {
            return new UpdateItemWithTypesContract(
                source.Name,
                source.Comment,
                source.QuantityType.Id,
                source.QuantityInPacket,
                source.QuantityInPacketType?.Id,
                source.ItemCategoryId.Value,
                source.ManufacturerId,
                source.ItemTypes.Select(ToUpdateItemTypeContract));
        }

        public UpdateItemTypeContract ToUpdateItemTypeContract(ItemType itemType)
        {
            return new UpdateItemTypeContract(
                itemType.Id,
                itemType.Name,
                itemType.Availabilities.Select(availabilityConverter.ToContract));
        }
    }
}