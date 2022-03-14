﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.TemporaryItems;

public class PermanentItem
{
    private readonly IEnumerable<IStoreItemAvailability> _availabilities;

    public PermanentItem(ItemId id, string name, string comment, QuantityType quantityType,
        float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket, ItemCategoryId itemCategoryId,
        ManufacturerId? manufacturerId, IEnumerable<IStoreItemAvailability> availabilities)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
        }

        Id = id;
        Name = name;
        Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        QuantityType = quantityType;
        QuantityInPacket = quantityInPacket;
        QuantityTypeInPacket = quantityTypeInPacket;
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        _availabilities = availabilities ?? throw new ArgumentNullException(nameof(availabilities));
    }

    public IReadOnlyCollection<IStoreItemAvailability> Availabilities => _availabilities.ToList().AsReadOnly();

    public ItemId Id { get; }
    public string Name { get; }
    public string Comment { get; }
    public QuantityType QuantityType { get; }
    public float QuantityInPacket { get; }
    public QuantityTypeInPacket QuantityTypeInPacket { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public ManufacturerId? ManufacturerId { get; }
}