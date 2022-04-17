﻿using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
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
        private readonly IToContractConverter<ItemAvailability, ItemAvailabilityContract> availabilityConverter;

        public MakeTemporaryItemPermanentContractConverter(
            IToContractConverter<ItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            this.availabilityConverter = availabilityConverter;
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
                source.Availabilities.Select(availabilityConverter.ToContract));
        }
    }
}