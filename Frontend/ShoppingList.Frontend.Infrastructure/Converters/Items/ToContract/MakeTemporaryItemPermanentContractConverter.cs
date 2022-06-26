using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class MakeTemporaryItemPermanentContractConverter :
        IToContractConverter<MakeTemporaryItemPermanentRequest, MakeTemporaryItemPermanentContract>
    {
        private readonly IToContractConverter<ItemAvailability, ItemAvailabilityContract> _availabilityConverter;

        public MakeTemporaryItemPermanentContractConverter(
            IToContractConverter<ItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            _availabilityConverter = availabilityConverter;
        }

        public MakeTemporaryItemPermanentContract ToContract(MakeTemporaryItemPermanentRequest source)
        {
            return new MakeTemporaryItemPermanentContract(
                source.Name,
                source.Comment,
                source.QuantityType,
                source.QuantityInPacket,
                source.QuantityTypeInPacket,
                source.ItemCategoryId,
                source.ManufacturerId,
                source.Availabilities.Select(_availabilityConverter.ToContract));
        }
    }
}