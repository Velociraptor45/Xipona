﻿using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

public class StoreItemReadModel
{
    public StoreItemReadModel(ItemId id, ItemName name, bool isDeleted, string comment, bool isTemporary,
        QuantityTypeReadModel quantityType, float quantityInPacket, QuantityTypeInPacketReadModel quantityTypeInPacket,
        ItemCategoryReadModel? itemCategory, ManufacturerReadModel? manufacturer,
        IEnumerable<StoreItemAvailabilityReadModel> availabilities, IEnumerable<ItemTypeReadModel> itemTypes)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
        Comment = comment;
        IsTemporary = isTemporary;
        QuantityType = quantityType ?? throw new ArgumentNullException(nameof(quantityType));
        QuantityInPacket = quantityInPacket;
        QuantityTypeInPacket = quantityTypeInPacket ?? throw new ArgumentNullException(nameof(quantityTypeInPacket));
        ItemCategory = itemCategory;
        Manufacturer = manufacturer;
        Availabilities = availabilities?.ToList() ?? throw new ArgumentNullException(nameof(availabilities));
        ItemTypes = itemTypes?.ToList() ?? throw new ArgumentNullException(nameof(itemTypes));
    }

    public ItemId Id { get; }
    public ItemName Name { get; }
    public bool IsDeleted { get; }
    public string Comment { get; }
    public bool IsTemporary { get; }
    public QuantityTypeReadModel QuantityType { get; }
    public float QuantityInPacket { get; }
    public QuantityTypeInPacketReadModel QuantityTypeInPacket { get; }
    public ItemCategoryReadModel? ItemCategory { get; }
    public ManufacturerReadModel? Manufacturer { get; }
    public IReadOnlyCollection<StoreItemAvailabilityReadModel> Availabilities { get; }
    public IReadOnlyCollection<ItemTypeReadModel> ItemTypes { get; }
}