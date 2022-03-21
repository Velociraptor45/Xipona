using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class MakeTemporaryItemPermanentContractConverter :
        IToContractConverter<MakeTemporaryItemPermanentRequest, MakeTemporaryItemPermanentContract>
    {
        private readonly IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter;

        public MakeTemporaryItemPermanentContractConverter(
            IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            this.availabilityConverter = availabilityConverter;
        }

        public MakeTemporaryItemPermanentContract ToContract(MakeTemporaryItemPermanentRequest source)
        {
            return new MakeTemporaryItemPermanentContract
            {
                Id = source.Id,
                Name = source.Name,
                Comment = source.Comment,
                QuantityType = source.QuantityType,
                QuantityInPacket = source.QuantityInPacket,
                QuantityTypeInPacket = source.QuantityTypeInPacket,
                ItemCategoryId = source.ItemCategoryId,
                ManufacturerId = source.ManufacturerId,
                Availabilities = source.Availabilities.Select(availabilityConverter.ToContract)
            };
        }
    }
}