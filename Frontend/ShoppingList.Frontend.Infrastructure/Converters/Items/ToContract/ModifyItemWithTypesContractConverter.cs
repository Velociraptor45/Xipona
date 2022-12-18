using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class ModifyItemWithTypesContractConverter :
        IToContractConverter<ModifyItemWithTypesRequest, ModifyItemWithTypesContract>
    {
        private readonly IToContractConverter<ItemAvailability, ItemAvailabilityContract> _availabilityConverter;

        public ModifyItemWithTypesContractConverter(
            IToContractConverter<ItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            _availabilityConverter = availabilityConverter;
        }

        public ModifyItemWithTypesContract ToContract(ModifyItemWithTypesRequest request)
        {
            var types = request.StoreItem.ItemTypes.Select(t => new ModifyItemTypeContract
            {
                Id = t.Id == Guid.Empty ? null : t.Id,
                Name = t.Name,
                Availabilities = t.Availabilities.Select(av => _availabilityConverter.ToContract(av))
            });

            return new ModifyItemWithTypesContract(
                request.StoreItem.Name,
                request.StoreItem.Comment,
                request.StoreItem.QuantityType.Id,
                request.StoreItem.QuantityInPacket,
                request.StoreItem.QuantityInPacketType?.Id,
                request.StoreItem.ItemCategoryId.Value,
                request.StoreItem.ManufacturerId,
                types);
        }
    }
}