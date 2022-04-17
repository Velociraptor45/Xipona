using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class UpdateItemContractConverter :
        IToContractConverter<Item, UpdateItemContract>
    {
        private readonly IToContractConverter<ItemAvailability, ItemAvailabilityContract> _availabilityConverter;

        public UpdateItemContractConverter(
            IToContractConverter<ItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            _availabilityConverter = availabilityConverter;
        }

        public UpdateItemContract ToContract(Item source)
        {
            return new UpdateItemContract(
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