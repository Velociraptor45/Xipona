﻿using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class ModifyItemWithTypesContractConverter :
        IToContractConverter<ModifyItemWithTypesRequest, ModifyItemWithTypesContract>
    {
        private readonly IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter;

        public ModifyItemWithTypesContractConverter(
            IToContractConverter<StoreItemAvailability, ItemAvailabilityContract> availabilityConverter)
        {
            this.availabilityConverter = availabilityConverter;
        }

        public ModifyItemWithTypesContract ToContract(ModifyItemWithTypesRequest request)
        {
            var types = request.StoreItem.ItemTypes.Select(t => new ModifyItemTypeContract
            {
                Id = t.Id == Guid.Empty ? null : t.Id,
                Name = t.Name,
                Availabilities = t.Availabilities.Select(av => availabilityConverter.ToContract(av))
            });

            return new ModifyItemWithTypesContract()
            {
                Id = request.StoreItem.Id,
                Name = request.StoreItem.Name,
                Comment = request.StoreItem.Comment,
                QuantityType = request.StoreItem.QuantityType.Id,
                QuantityInPacket = request.StoreItem.QuantityInPacket,
                QuantityTypeInPacket = request.StoreItem.QuantityInPacketType?.Id,
                ItemCategoryId = request.StoreItem.ItemCategoryId.Value,
                ManufacturerId = request.StoreItem.ManufacturerId,
                ItemTypes = types
            };
        }
    }
}