using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class ModifyItemWithTypesContractConverter
    {
        public ModifyItemWithTypesContract ToContract(ModifyItemWithTypesRequest request)
        {
            var availabilityConverter = new ItemAvailabilityContractConverter();
            var types = request.StoreItem.ItemTypes.Select(t => new ModifyItemTypeContract
            {
                Id = t.Id == Guid.Empty ? null : t.Id,
                Name = t.Name,
                Availabilities = t.Availabilities.Select(av => availabilityConverter.ToContract(av))
            });

            return new ModifyItemWithTypesContract()
            {
                Id = request.StoreItem.Id,
                Name = request.StoreItem.Name,
                Comment = request.StoreItem.Comment,
                QuantityType = request.StoreItem.QuantityType.Id,
                QuantityInPacket = request.StoreItem.QuantityInPacket,
                QuantityTypeInPacket = request.StoreItem.QuantityInPacketType.Id,
                ItemCategoryId = request.StoreItem.ItemCategoryId.Value,
                ManufacturerId = request.StoreItem.ManufacturerId,
                ItemTypes = types
            };
        }
    }
}