﻿using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class ItemConverter : IToDomainConverter<ItemContract, Item>
    {
        private readonly IToDomainConverter<ItemAvailabilityContract, ItemAvailability> _availabilityConverter;
        //private readonly IToDomainConverter<QuantityTypeContract, QuantityType> _quantityTypeConverter;
        //private readonly IToDomainConverter<QuantityTypeInPacketContract, QuantityTypeInPacket> _quantityTypeInPacketConverter;

        public ItemConverter(
            IToDomainConverter<ItemAvailabilityContract, ItemAvailability> availabilityConverter
            //IToDomainConverter<QuantityTypeContract, QuantityType> quantityTypeConverter,
            //IToDomainConverter<QuantityTypeInPacketContract, QuantityTypeInPacket> quantityTypeInPacketConverter
            )
        {
            _availabilityConverter = availabilityConverter;
            //_quantityTypeConverter = quantityTypeConverter;
            //_quantityTypeInPacketConverter = quantityTypeInPacketConverter;
        }

        public Item ToDomain(ItemContract source)
        {
            return new Item(
                source.Id,
                source.Name,
                source.IsDeleted,
                source.Comment,
                source.IsTemporary,
                null, //_quantityTypeConverter.ToDomain(source.QuantityType), // todo
                source.QuantityInPacket,
                null,
                //source.QuantityTypeInPacket is null ?
                //    null :
                //    _quantityTypeInPacketConverter.ToDomain(source.QuantityTypeInPacket),
                source.ItemCategory?.Id,
                source.Manufacturer?.Id,
                source.Availabilities.Select(_availabilityConverter.ToDomain),
                source.ItemTypes.Select(CreateItemType));
        }

        private ItemType CreateItemType(ItemTypeContract contract)
        {
            return new ItemType(
                contract.Id,
                contract.Name,
                contract.Availabilities.Select(_availabilityConverter.ToDomain));
        }
    }
}