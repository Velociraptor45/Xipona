﻿using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;

public class ItemWithTypesUpdate
{
    public ItemWithTypesUpdate(ItemId oldId, ItemName name, string comment,
        QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId,
        IEnumerable<ItemTypeUpdate> typeUpdates)
    {
        OldId = oldId;
        Name = name;
        Comment = comment;
        QuantityType = quantityType;
        QuantityInPacket = quantityInPacket;
        QuantityTypeInPacket = quantityTypeInPacket;
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        TypeUpdates = typeUpdates?.ToList() ?? throw new ArgumentNullException(nameof(typeUpdates));
    }

    public ItemId OldId { get; }
    public ItemName Name { get; }
    public string Comment { get; }
    public QuantityType QuantityType { get; }
    public float QuantityInPacket { get; }
    public QuantityTypeInPacket QuantityTypeInPacket { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public ManufacturerId? ManufacturerId { get; }
    public IReadOnlyCollection<ItemTypeUpdate> TypeUpdates { get; }
}