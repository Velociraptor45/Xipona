﻿using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems
{
    public class PermanentItemConverter : IToDomainConverter<MakeTemporaryItemPermanentContract, PermanentItem>
    {
        private readonly IToDomainConverter<ItemAvailabilityContract, ShortAvailability> shortAvailabilityConverter;

        public PermanentItemConverter(
            IToDomainConverter<ItemAvailabilityContract, ShortAvailability> shortAvailabilityConverter)
        {
            this.shortAvailabilityConverter = shortAvailabilityConverter;
        }

        public PermanentItem ToDomain(MakeTemporaryItemPermanentContract source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new PermanentItem(
                new StoreItemId(source.Id),
                source.Name,
                source.Comment,
                source.QuantityType.ToEnum<QuantityType>(),
                source.QuantityInPacket,
                source.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>(),
                new ItemCategoryId(source.ItemCategoryId),
                source.ManufacturerId.HasValue ?
                    new ManufacturerId(source.ManufacturerId.Value) :
                    null,
                shortAvailabilityConverter.ToDomain(source.Availabilities));
        }
    }
}