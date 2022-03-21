using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class ModifyItemContractConverter :
        IToContractConverter<StoreItem, ModifyItemContract>
    {
        private readonly IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter;

        public ModifyItemContractConverter(
            IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            this.availabilityConverter = availabilityConverter;
        }

        public ModifyItemContract ToContract(StoreItem source)
        {
            return new ModifyItemContract
            {
                Id = source.Id,
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