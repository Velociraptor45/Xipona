using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class ModifyItemWithTypesContractConverter :
        IToContractConverter<ModifyItemWithTypesRequest, ModifyItemWithTypesContract>
    {
        private readonly IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter;

        public ModifyItemWithTypesContractConverter(
            IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            this.availabilityConverter = availabilityConverter;
        }

        public ModifyItemWithTypesContract ToContract(ModifyItemWithTypesRequest request)
        {
            var types = request.StoreItem.ItemTypes.Select(t => new ModifyItemTypeContract
            {
                Id = t.Id == Guid.Empty ? null : t.Id,
                Name = t.Name,
                Availabilities = t.Availabilities.Select(av => availabilityConverter.ToContract(av))
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