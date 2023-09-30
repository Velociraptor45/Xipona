using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class ModifyItemWithTypesContractConverter :
        IToContractConverter<EditedItem, ModifyItemWithTypesContract>
    {
        private readonly IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> _availabilityConverter;

        public ModifyItemWithTypesContractConverter(
            IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            _availabilityConverter = availabilityConverter;
        }

        public ModifyItemWithTypesContract ToContract(EditedItem item)
        {
            var types = item.ItemTypes.Select(t => new ModifyItemTypeContract
            {
                Id = t.Id == Guid.Empty ? null : t.Id,
                Name = t.Name,
                Availabilities = t.Availabilities.Select(av => _availabilityConverter.ToContract(av))
            });

            return new ModifyItemWithTypesContract(
                item.Name,
                item.Comment,
                item.QuantityType.Id,
                item.QuantityInPacket,
                item.QuantityInPacketType?.Id,
                item.ItemCategoryId.Value,
                item.ManufacturerId,
                types);
        }
    }
}