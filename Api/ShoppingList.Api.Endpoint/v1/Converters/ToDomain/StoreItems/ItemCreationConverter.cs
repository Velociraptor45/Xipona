﻿using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems;

public class ItemCreationConverter : IToDomainConverter<CreateItemContract, ItemCreation>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> _storeItemAvailabilityConverter;

    public ItemCreationConverter(
        IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> storeItemAvailabilityConverter)
    {
        _storeItemAvailabilityConverter = storeItemAvailabilityConverter;
    }

    public ItemCreation ToDomain(CreateItemContract source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new ItemCreation(
            source.Name,
            source.Comment,
            source.QuantityType.ToEnum<QuantityType>(),
            source.QuantityInPacket,
            source.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>(),
            new ItemCategoryId(source.ItemCategoryId),
            source.ManufacturerId.HasValue ?
                new ManufacturerId(source.ManufacturerId.Value) :
                null,
            _storeItemAvailabilityConverter.ToDomain(source.Availabilities));
    }
}