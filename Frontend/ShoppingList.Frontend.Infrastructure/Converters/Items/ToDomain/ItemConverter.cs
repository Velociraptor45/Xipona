﻿using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class ItemConverter : IToDomainConverter<StoreItemContract, StoreItem>
    {
        private readonly IToDomainConverter<StoreItemAvailabilityContract, StoreItemAvailability> availabilityConverter;
        private readonly IToDomainConverter<QuantityTypeContract, QuantityType> quantityTypeConverter;
        private readonly IToDomainConverter<QuantityTypeInPacketContract, QuantityTypeInPacket> quantityTypeInPacketConverter;

        public ItemConverter(
            IToDomainConverter<StoreItemAvailabilityContract, StoreItemAvailability> availabilityConverter,
            IToDomainConverter<QuantityTypeContract, QuantityType> quantityTypeConverter,
            IToDomainConverter<QuantityTypeInPacketContract, QuantityTypeInPacket> quantityTypeInPacketConverter)
        {
            this.availabilityConverter = availabilityConverter;
            this.quantityTypeConverter = quantityTypeConverter;
            this.quantityTypeInPacketConverter = quantityTypeInPacketConverter;
        }

        public StoreItem ToDomain(StoreItemContract source)
        {
            return new StoreItem(
                source.Id,
                source.Name,
                source.IsDeleted,
                source.Comment,
                source.IsTemporary,
                quantityTypeConverter.ToDomain(source.QuantityType),
                source.QuantityInPacket,
                source.QuantityTypeInPacket is null ?
                    null :
                    quantityTypeInPacketConverter.ToDomain(source.QuantityTypeInPacket),
                source.ItemCategory?.Id,
                source.Manufacturer?.Id,
                source.Availabilities.Select(availabilityConverter.ToDomain),
                source.ItemTypes.Select(CreateItemType));
        }

        private ItemType CreateItemType(ItemTypeContract contract)
        {
            return new ItemType(
                contract.Id,
                contract.Name,
                contract.Availabilities.Select(availabilityConverter.ToDomain));
        }
    }
}