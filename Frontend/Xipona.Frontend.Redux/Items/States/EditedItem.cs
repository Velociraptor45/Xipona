﻿using QuantityType = ProjectHermes.Xipona.Frontend.Redux.Shared.States.QuantityType;
using QuantityTypeInPacket = ProjectHermes.Xipona.Frontend.Redux.Shared.States.QuantityTypeInPacket;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.States;

public record EditedItem(
    Guid Id,
    string Name,
    bool IsDeleted,
    string Comment,
    bool IsTemporary,
    QuantityType QuantityType,
    float? QuantityInPacket,
    QuantityTypeInPacket? QuantityInPacketType,
    Guid? ItemCategoryId,
    Guid? ManufacturerId,
    IReadOnlyCollection<EditedItemAvailability> Availabilities,
    IReadOnlyCollection<EditedItemType> ItemTypes,
    ItemMode ItemMode) : IAvailable
{
    public bool IsItemWithTypes => ItemMode == ItemMode.WithTypes;
}