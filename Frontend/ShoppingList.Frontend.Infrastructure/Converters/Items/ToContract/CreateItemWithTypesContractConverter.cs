using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.Items.States;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class CreateItemWithTypesContractConverter :
        IToContractConverter<EditedItem, CreateItemWithTypesContract>
    {
        private readonly IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> _availabilityConverter;

        public CreateItemWithTypesContractConverter(
            IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            _availabilityConverter = availabilityConverter;
        }

        public CreateItemWithTypesContract ToContract(EditedItem source)
        {
            return new CreateItemWithTypesContract
            {
                Name = source.Name,
                Comment = source.Comment,
                QuantityType = source.QuantityType.Id,
                QuantityInPacket = source.QuantityInPacket,
                QuantityTypeInPacket = source.QuantityInPacketType?.Id,
                ItemCategoryId = source.ItemCategoryId.Value,
                ManufacturerId = source.ManufacturerId,
                ItemTypes = source.ItemTypes.Select(ToCreateItemTypeContract)
            };
        }

        private CreateItemTypeContract ToCreateItemTypeContract(EditedItemType itemType)
        {
            return new CreateItemTypeContract
            {
                Name = itemType.Name,
                Availabilities = itemType.Availabilities.Select(_availabilityConverter.ToContract)
            };
        }
    }
}