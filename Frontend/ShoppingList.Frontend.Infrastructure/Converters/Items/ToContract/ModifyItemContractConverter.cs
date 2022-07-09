using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.ModifyItem;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class ModifyItemContractConverter :
        IToContractConverter<Item, ModifyItemContract>
    {
        private readonly IToContractConverter<ItemAvailability, ItemAvailabilityContract> _availabilityConverter;

        public ModifyItemContractConverter(
            IToContractConverter<ItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            _availabilityConverter = availabilityConverter;
        }

        public ModifyItemContract ToContract(Item source)
        {
            return new ModifyItemContract(
                source.Name,
                source.Comment,
                source.QuantityType.Id,
                source.QuantityInPacket,
                source.QuantityInPacketType?.Id,
                source.ItemCategoryId.Value,
                source.ManufacturerId,
                source.Availabilities.Select(_availabilityConverter.ToContract));
        }
    }
}