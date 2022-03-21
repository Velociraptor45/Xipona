﻿using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class UpdateItemWithTypesContractConverter :
        IToContractConverter<StoreItem, UpdateItemWithTypesContract>
    {
        private readonly IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter;

        public UpdateItemWithTypesContractConverter(
            IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            this.availabilityConverter = availabilityConverter;
        }

        public UpdateItemWithTypesContract ToContract(StoreItem source)
        {
            return new UpdateItemWithTypesContract
            {
                OldId = source.Id,
                Name = source.Name,
                Comment = source.Comment,
                QuantityType = source.QuantityType.Id,
                QuantityInPacket = source.QuantityInPacket,
                QuantityTypeInPacket = source.QuantityInPacketType?.Id,
                ItemCategoryId = source.ItemCategoryId.Value,
                ManufacturerId = source.ManufacturerId,
                ItemTypes = source.ItemTypes.Select(ToUpdateItemTypeContract)
            };
        }

        public UpdateItemTypeContract ToUpdateItemTypeContract(ItemType itemType)
        {
            return new UpdateItemTypeContract
            {
                OldId = itemType.Id,
                Name = itemType.Name,
                Availabilities = itemType.Availabilities.Select(availabilityConverter.ToContract)
            };
        }
    }
}