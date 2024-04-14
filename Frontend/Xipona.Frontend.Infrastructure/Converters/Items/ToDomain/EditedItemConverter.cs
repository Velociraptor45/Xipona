﻿using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using System;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class EditedItemConverter : IToDomainConverter<ItemContract, EditedItem>
    {
        private readonly IToDomainConverter<ItemAvailabilityContract, EditedItemAvailability> _availabilityConverter;
        private readonly IToDomainConverter<QuantityTypeContract, QuantityType> _quantityTypeConverter;
        private readonly IToDomainConverter<QuantityTypeInPacketContract, QuantityTypeInPacket> _quantityTypeInPacketConverter;

        public EditedItemConverter(
            IToDomainConverter<ItemAvailabilityContract, EditedItemAvailability> availabilityConverter,
            IToDomainConverter<QuantityTypeContract, QuantityType> quantityTypeConverter,
            IToDomainConverter<QuantityTypeInPacketContract, QuantityTypeInPacket> quantityTypeInPacketConverter)
        {
            _availabilityConverter = availabilityConverter;
            _quantityTypeConverter = quantityTypeConverter;
            _quantityTypeInPacketConverter = quantityTypeInPacketConverter;
        }

        public EditedItem ToDomain(ItemContract source)
        {
            var types = source.ItemTypes.Select(CreateItemType).ToList();

            return new EditedItem(
                source.Id,
                source.Name,
                source.IsDeleted,
                source.Comment,
                source.IsTemporary,
                _quantityTypeConverter.ToDomain(source.QuantityType),
                source.QuantityInPacket,
                source.QuantityTypeInPacket is null ?
                    null :
                    _quantityTypeInPacketConverter.ToDomain(source.QuantityTypeInPacket),
                source.ItemCategory?.Id,
                source.Manufacturer?.Id,
                source.Availabilities.Select(_availabilityConverter.ToDomain).ToList(),
                types,
                types.Count > 0 ? ItemMode.WithTypes : ItemMode.WithoutTypes);
        }

        private EditedItemType CreateItemType(ItemTypeContract contract)
        {
            return new EditedItemType(
                contract.Id,
                Guid.NewGuid(),
                contract.Name,
                contract.Availabilities.Select(_availabilityConverter.ToDomain).ToList());
        }
    }
}