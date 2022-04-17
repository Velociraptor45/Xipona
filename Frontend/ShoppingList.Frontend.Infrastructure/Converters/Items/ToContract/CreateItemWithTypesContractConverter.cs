using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class CreateItemWithTypesContractConverter :
        IToContractConverter<Item, CreateItemWithTypesContract>
    {
        private readonly IToContractConverter<ItemAvailability, ItemAvailabilityContract> availabilityConverter;

        public CreateItemWithTypesContractConverter(
            IToContractConverter<ItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            this.availabilityConverter = availabilityConverter;
        }

        public CreateItemWithTypesContract ToContract(Item source)
        {
            return new CreateItemWithTypesContract()
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

        private CreateItemTypeContract ToCreateItemTypeContract(ItemType itemType)
        {
            return new CreateItemTypeContract()
            {
                Name = itemType.Name,
                Availabilities = itemType.Availabilities.Select(availabilityConverter.ToContract)
            };
        }
    }
}