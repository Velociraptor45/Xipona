﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ItemUpdateWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems;

public class UpdateItemWithTypesCommandConverter
    : IToDomainConverter<UpdateItemWithTypesContract, UpdateItemWithTypesCommand>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> _availabilityConverter;

    public UpdateItemWithTypesCommandConverter(
        IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public UpdateItemWithTypesCommand ToDomain(UpdateItemWithTypesContract source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        var itemTypeUpdates = source.ItemTypes.Select(t => new ItemTypeUpdate(
            new ItemTypeId(t.OldId),
            new ItemTypeName(t.Name),
            _availabilityConverter.ToDomain(t.Availabilities)));

        var itemUpdate = new ItemWithTypesUpdate(
            new ItemId(source.OldId),
            new ItemName(source.Name),
            new Comment(source.Comment),
            new ItemQuantity(
                source.QuantityType.ToEnum<QuantityType>(),
                new ItemQuantityInPacket(
                    new Quantity(source.QuantityInPacket),
                    source.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>())),
            new ItemCategoryId(source.ItemCategoryId),
            source.ManufacturerId.HasValue ? new ManufacturerId(source.ManufacturerId.Value) : null,
            itemTypeUpdates);

        return new UpdateItemWithTypesCommand(itemUpdate);
    }
}