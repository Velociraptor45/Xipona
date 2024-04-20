﻿using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItem;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class UpdateItemContractConverter : IToContractConverter<EditedItem, UpdateItemContract>
    {
        private readonly IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> _availabilityConverter;

        public UpdateItemContractConverter(
            IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            _availabilityConverter = availabilityConverter;
        }

        public UpdateItemContract ToContract(EditedItem source)
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