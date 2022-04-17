using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class UpdateItemWithTypesContractConverter :
        IToContractConverter<Item, UpdateItemWithTypesContract>
    {
        private readonly IToContractConverter<ItemAvailability, ItemAvailabilityContract> _availabilityConverter;

        public UpdateItemWithTypesContractConverter(
            IToContractConverter<ItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            _availabilityConverter = availabilityConverter;
        }

        public UpdateItemWithTypesContract ToContract(Item source)
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
                itemType.Availabilities.Select(_availabilityConverter.ToContract));
        }
    }
}