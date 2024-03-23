using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

public record ShoppingListItem(
    ShoppingListItemId Id,
    Guid? TypeId,
    string Name,
    bool IsTemporary,
    float PricePerQuantity,
    QuantityType QuantityType,
    float? QuantityInPacket,
    QuantityTypeInPacket? QuantityInPacketType,
    string ItemCategory,
    string Manufacturer,
    bool IsInBasket,
    float Quantity,
    bool Hidden);