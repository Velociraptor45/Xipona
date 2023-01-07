using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.Items.States;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class ModifyItemWithTypesContractConverter :
        IToContractConverter<ModifyItemWithTypesRequest, ModifyItemWithTypesContract>
    {
        private readonly IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> _availabilityConverter;

        public ModifyItemWithTypesContractConverter(
            IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            _availabilityConverter = availabilityConverter;
        }

        public ModifyItemWithTypesContract ToContract(ModifyItemWithTypesRequest request)
        {
            var types = request.Item.ItemTypes.Select(t => new ModifyItemTypeContract
            {
                Id = t.Id == Guid.Empty ? null : t.Id,
                Name = t.Name,
                Availabilities = t.Availabilities.Select(av => _availabilityConverter.ToContract(av))
            });

            return new ModifyItemWithTypesContract(
                request.Item.Name,
                request.Item.Comment,
                request.Item.QuantityType.Id,
                request.Item.QuantityInPacket,
                request.Item.QuantityInPacketType?.Id,
                request.Item.ItemCategoryId.Value,
                request.Item.ManufacturerId,
                types);
        }
    }
}