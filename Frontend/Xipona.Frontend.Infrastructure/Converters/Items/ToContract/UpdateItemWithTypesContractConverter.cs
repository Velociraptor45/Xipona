﻿using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemWithTypes;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class UpdateItemWithTypesContractConverter :
        IToContractConverter<EditedItem, UpdateItemWithTypesContract>
    {
        private readonly IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> _availabilityConverter;

        public UpdateItemWithTypesContractConverter(
            IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            _availabilityConverter = availabilityConverter;
        }

        public UpdateItemWithTypesContract ToContract(EditedItem source)
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

        public UpdateItemTypeContract ToUpdateItemTypeContract(EditedItemType itemType)
        {
            return new UpdateItemTypeContract(
                itemType.Id,
                itemType.Name,
                itemType.Availabilities.Select(_availabilityConverter.ToContract));
        }
    }
}